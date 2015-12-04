using System;
using System.IdentityModel.Tokens;
using MakingSense.AspNet.Authentication.SimpleToken;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Builder
{
	public static class SimpleTokenAppBuilderExtensions
	{
		public static IApplicationBuilder UseSimpleTokenAuthentication([NotNull] this IApplicationBuilder app, Action<SimpleTokenAuthenticationOptions> configureOptions = null, string authenticationScheme = "Bearer")
		{
			var options = new SimpleTokenAuthenticationOptions()
			{
				AuthenticationScheme = authenticationScheme
			};

			if (configureOptions != null)
			{
				configureOptions(options);
			}

			if (options.SecurityTokenValidatorsFactory == null)
			{
				options.SecurityTokenValidatorsFactory = () => app.ApplicationServices.GetServices<ISecurityTokenValidator>();
			}

			return app.UseMiddleware<SimpleTokenAuthenticationMiddleware>(options);
		}
	}
}
