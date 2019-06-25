using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MakingSense.AspNetCore.Authentication.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace MakingSense.AspNetCore.Authentication.SimpleToken
{
	public class SimpleTokenAuthenticationHandler : AuthenticationHandler<SimpleTokenAuthenticationOptions>
	{
		public SimpleTokenAuthenticationHandler(
			IOptionsMonitor<SimpleTokenAuthenticationOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		/// <summary>
		/// Overrides the standard AuthenticationHandler to be more robust supporting [RFC 6750](http://tools.ietf.org/html/rfc6750) and
		/// some licenses based on [GitHub behavior](https://developer.github.com/v3/oauth/#use-the-access-token-to-access-the-api).
		/// </summary>
		/// <remarks>
		/// It does not search in Form-Encoded Body Parameter (http://tools.ietf.org/html/rfc6750#section-2.2).
		/// </remarks>
		/// <returns>
		/// Returns Token if found, null otherwise.
		/// </returns>
		/// <exception>
		/// Throws AuthenticationException when Authentication header is found, but id does not contains valid data format.
		/// </exception>
		public static string ExtractToken(HttpRequest request)
		{
			var authorizationHeader = (string)request.Headers[HeaderNames.Authorization];
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
				// Not so nice, but AuthenticateResult.Fail does not allow us to show the error
				throw new AuthenticationException("Authorization header exists but does not contains valid information.");
			}

			var tokenFromQuery = (string)request.Query["access_token"] ?? request.Query["api_key"];
			if (tokenFromQuery != null)
			{
				return tokenFromQuery.Trim();
			}

			return null;
		}

		static readonly Task DoneTask = Task.FromResult(0);

		/// <summary>
		/// Searches the 'Authorization' header for a 'Bearer' token. If the 'Bearer' token is found, it is validated using <see cref="TokenValidationParameters"/> set in the options.
		/// </summary>
		/// <returns></returns>
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			string token;
			try
			{
				token = ExtractToken(Request);
			}
			catch (AuthenticationException ex)
			{
				return AuthenticateResult.Fail(ex.Message);
			}	
			// If no token found, no further work possible
			if (string.IsNullOrEmpty(token))
			{
				return AuthenticateResult.NoResult();
			}

			var validationParameters = Options.TokenValidationParameters.Clone();

			var validators = Options.SecurityTokenValidatorsFactory();
			foreach (var validator in validators)
			{
				if (validator.CanReadToken(token))
				{
					var principal = validator.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
					var ticket = new AuthenticationTicket(principal, Scheme.Name);
					return AuthenticateResult.Success(ticket);
				}
			}

				// Ugly patch to make this method should to be async in order to allow result caching by caller
				await DoneTask;

			return AuthenticateResult.Fail("Authorization token has been detected but it cannot be read.");
		}
	}
}
