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
			return app.UseMiddleware<SimpleTokenAuthenticationMiddleware>(
				new ConfigureOptions<SimpleTokenAuthenticationOptions>(o =>
				{
					if (configureOptions != null)
					{
						configureOptions(o);
					}
					if (o.SecurityTokenValidatorsFactory == null)
					{
						o.SecurityTokenValidatorsFactory = () => app.ApplicationServices.GetServices<ISecurityTokenValidator>();
					}
				})
				{
					Name = optionsName
				});
		}
	}
}
