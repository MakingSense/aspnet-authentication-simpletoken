using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Framework.Internal;

namespace MakingSense.AspNetCore.Authentication.SimpleToken
{
	/// <summary>
	/// SimpleToken authentication middleware component which is added to an HTTP pipeline. This class is not
	/// created by application code directly, instead it is added by calling the the IAppBuilder UseSimpleTokenAuthentication
	/// extension method.
	/// </summary>
	public class SimpleTokenAuthenticationMiddleware : AuthenticationMiddleware<SimpleTokenAuthenticationOptions>
	{
		/// <summary>
		/// SimpleToken authentication component which is added to an HTTP pipeline. This constructor is not
		/// called by application code directly, instead it is added by calling the the IAppBuilder UseSimpleTokenAuthentication
		/// extension method.
		/// </summary>
		public SimpleTokenAuthenticationMiddleware(
			[NotNull] RequestDelegate next,
			[NotNull] ILoggerFactory loggerFactory,
			[NotNull] UrlEncoder encoder,
			[NotNull] IOptions<SimpleTokenAuthenticationOptions> options)
			: base(next, options, loggerFactory, encoder)
		{
			if (next == null)
			{
				throw new ArgumentNullException(nameof(next));
			}

			if (loggerFactory == null)
			{
				throw new ArgumentNullException(nameof(loggerFactory));
			}

			if (encoder == null)
			{
				throw new ArgumentNullException(nameof(encoder));
			}

			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}


			if (string.IsNullOrEmpty(Options.AuthenticationScheme))
			{
				throw new ArgumentException(nameof(Options.AuthenticationScheme));
			}

			if (Options.SecurityTokenValidatorsFactory == null)
			{
				throw new ArgumentException(nameof(Options.SecurityTokenValidatorsFactory));
			}
		}

		/// <summary>
		/// Called by the AuthenticationMiddleware base class to create a per-request handler.
		/// </summary>
		/// <returns>A new instance of the request handler</returns>
		protected override AuthenticationHandler<SimpleTokenAuthenticationOptions> CreateHandler()
		{
			return new SimpleTokenAuthenticationHandler();
		}
	}
}
