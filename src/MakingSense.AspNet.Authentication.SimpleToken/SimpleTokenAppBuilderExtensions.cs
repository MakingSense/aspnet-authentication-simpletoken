using System;
using MakingSense.AspNet.Authentication.SimpleToken;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;
using System.IdentityModel.Tokens;

namespace Microsoft.AspNet.Builder
{
	public static class SimpleTokenAppBuilderExtensions
	{
		public static IApplicationBuilder UseSimpleTokenAuthentication([NotNull] this IApplicationBuilder app, Action<SimpleTokenAuthenticationOptions> configureOptions = null, string optionsName = "")
		{
			var options = new SimpleTokenAuthenticationOptions()
			{
				AuthenticationScheme = optionsName
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
