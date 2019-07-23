using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
							var serviceProvider = builder.Services.BuildServiceProvider();
							var httpContext = serviceProvider.GetService<IHttpContextAccessor>().HttpContext;
							return httpContext.RequestServices.GetServices<ISecurityTokenValidator>();
						};
					}
				});
		}
	}
}
