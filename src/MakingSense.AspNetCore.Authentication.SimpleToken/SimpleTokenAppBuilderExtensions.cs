using System;
using MakingSense.AspNetCore.Authentication.SimpleToken;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
	public static class SimpleTokenAppBuilderExtensions
	{
		public static IApplicationBuilder UseSimpleTokenAuthentication(this IApplicationBuilder app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			return UseSimpleTokenAuthentication(app, new SimpleTokenAuthenticationOptions());
		}

		public static IApplicationBuilder UseSimpleTokenAuthentication(this IApplicationBuilder app, SimpleTokenAuthenticationOptions options)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			if (string.IsNullOrEmpty(options.AuthenticationScheme))
			{
				options.AuthenticationScheme = "Bearer";
			}

			if (options.SecurityTokenValidatorsFactory == null)
			{
				// TODO: fix it because it is using app services, and it should use scope services,
				// a work around could be:
				// ```
				// SecurityTokenValidatorsFactory = () =>
				// {
				//     var context = app.ApplicationServices.GetService<IHttpContextAccessor>().HttpContext;
				//     return context.RequestServices.GetServices<ISecurityTokenValidator>();
				// }
				// ```
				options.SecurityTokenValidatorsFactory = () => app.ApplicationServices.GetServices<ISecurityTokenValidator>();
			}

			return app.UseMiddleware<SimpleTokenAuthenticationMiddleware>(Options.Create(options));
		}
	}
}
