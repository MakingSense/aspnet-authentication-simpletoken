using System;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Microsoft.Framework.Internal;

namespace MakingSense.AspNet.Authentication.SimpleToken
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
			[NotNull] SimpleTokenAuthenticationOptions options,
			[NotNull] ILoggerFactory loggerFactory,
			[NotNull] IUrlEncoder encoder)
			: base(next, options, loggerFactory, encoder)
		{
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
