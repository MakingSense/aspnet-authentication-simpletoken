using System;
using Microsoft.AspNet.Http.Authentication;
using System.IdentityModel.Tokens;
using Microsoft.AspNet.Http;
using System.Text;
using System.Threading.Tasks;
using MakingSense.AspNet.Authentication.Abstractions;
using Microsoft.AspNet.Authentication;

namespace MakingSense.AspNet.Authentication.SimpleToken
{
	public class SimpleTokenAuthenticationHandler : AuthenticationHandler<SimpleTokenAuthenticationOptions>
	{
		/// <summary>
		/// Overrides the standard AuthenticationHandler to be more robust supporting [RFC 6750](http://tools.ietf.org/html/rfc6750) and
		/// some licenses based on [GitHub behavior](https://developer.github.com/v3/oauth/#use-the-access-token-to-access-the-api).
		/// </summary>
		/// <remarks>
		/// It does not search in Form-Encoded Body Parameter (http://tools.ietf.org/html/rfc6750#section-2.2).
		/// </remarks>
		/// <returns>
		/// Returns Token if found, null otherwise
		/// </returns>
		public static string ExtractToken(HttpRequest request)
		{
			var authorizationHeader = request.Headers.Get("Authorization");
			if (authorizationHeader != null)
			{
				// Search in Authorization Request Header Field (http://tools.ietf.org/html/rfc6750#section-2.1)
				// Also as GitHub we accept `Bearer`, `OAuth2` and `Token` (https://developer.github.com/v3/oauth/#normalized-scopes)
				var acceptedSchemes = new[] { "Bearer ", "OAuth2 ", "Token " };
				foreach (var scheme in acceptedSchemes)
				{
					if (authorizationHeader.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
					{
						return authorizationHeader.Substring(scheme.Length).Trim();
					}
				}

				// Search for basic authentication.
				// As GitHub we accept basic authentication reading only the secret part (https://developer.github.com/v3/auth/#via-oauth-tokens)
				if (authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
				{
					var pair = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeader.Substring("Basic ".Length)));
					var ix = pair.IndexOf(':');
					if (ix >= 0)
					{
						return pair.Substring(ix + 1).Trim();
					}
				}

				throw new AuthenticationException("Authorization header exists but does not contains valid information.");
			}

			// Search in URI Query Parameter (http://tools.ietf.org/html/rfc6750#section-2.3)
			var tokenFromQuery = request.Query["access_token"] ?? request.Query["api_key"];
			if (tokenFromQuery != null)
			{
				return tokenFromQuery.Trim();
			}

			return null;
		}


		/// <summary>
		/// Searches the 'Authorization' header for a 'Bearer' token. If the 'Bearer' token is found, it is validated using <see cref="TokenValidationParameters"/> set in the options.
		/// </summary>
		/// <returns></returns>
		protected override Task<AuthenticationTicket> HandleAuthenticateAsync()
		{
			string token = ExtractToken(Request);

			// If no token found, no further work possible
			if (string.IsNullOrEmpty(token))
			{
				return Task.FromResult<AuthenticationTicket>(null);
			}

			var validationParameters = Options.TokenValidationParameters.Clone();

			SecurityToken validatedToken;
			var validators = Options.SecurityTokenValidatorsFactory();
			foreach (var validator in validators)
			{
				if (validator.CanReadToken(token))
				{
					var principal = validator.ValidateToken(token, validationParameters, out validatedToken);
					var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Options.AuthenticationScheme);
					return Task.FromResult(ticket);
				}
			}

			throw new AuthenticationException("Authorization token has been detected but it cannot be read.");
		}
	}
}
