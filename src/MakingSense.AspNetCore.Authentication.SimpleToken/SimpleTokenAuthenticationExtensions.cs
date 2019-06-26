using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace MakingSense.AspNetCore.Authentication.SimpleToken
{
	public static class SimpleTokenAuthenticationExtensions
	{
		public static AuthenticationBuilder AddSimpleTokenAuthentication(this AuthenticationBuilder builder)
			=> builder.AddSimpleTokenAuthentication(SimpleTokenDefaults.AuthenticationScheme, null);

		public static AuthenticationBuilder AddSimpleTokenAuthentication(this AuthenticationBuilder builder,
			Action<SimpleTokenAuthenticationOptions> configureOptions)
			=> builder.AddSimpleTokenAuthentication(SimpleTokenDefaults.AuthenticationScheme, configureOptions);

		public static AuthenticationBuilder AddSimpleTokenAuthentication(this AuthenticationBuilder builder,
			string authenticationScheme,
			Action<SimpleTokenAuthenticationOptions> configureOptions)
			=> builder.AddSimpleTokenAuthentication(authenticationScheme, SimpleTokenDefaults.DisplayName, configureOptions: configureOptions);

		public static AuthenticationBuilder AddSimpleTokenAuthentication(this AuthenticationBuilder builder,
			string authenticationScheme,
			string displayName,
			Action<SimpleTokenAuthenticationOptions> configureOptions)
		{
			return builder.AddScheme<SimpleTokenAuthenticationOptions, SimpleTokenAuthenticationHandler>(authenticationScheme, displayName,
				(SimpleTokenAuthenticationOptions options) => {
					configureOptions?.Invoke(options);
					
					if (options.SecurityTokenValidatorsFactory == null)
					{
						options.SecurityTokenValidatorsFactory = () =>
						{
							// TODO: fix it because it is using app services, and it should use scope services,
							// a work around could be:
							// ```
							// SecurityTokenValidatorsFactory = () =>
							// {
							//     var context = builder.Services.BuildServiceProvider().GetService<IHttpContextAccessor>().HttpContext;
							//     return context.RequestServices.GetServices<ISecurityTokenValidator>();
							// }
							// ```
							var serviceProvider = builder.Services.BuildServiceProvider();
							return serviceProvider.GetServices<ISecurityTokenValidator>();
						};
					}
				});
		}
	}
}
