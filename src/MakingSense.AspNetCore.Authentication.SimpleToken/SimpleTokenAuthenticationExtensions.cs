using Microsoft.AspNetCore.Authentication;
using System;

namespace MakingSense.AspNetCore.Authentication.SimpleToken
{
	public static class SimpleTokenAuthenticationExtensions
	{
		public static AuthenticationBuilder AddSimpleTokenAuthentication(this AuthenticationBuilder builder)
			=> builder.AddSimpleTokenAuthentication(SimpleTokenDefaults.AuthenticationScheme, _ => { });

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
			return builder.AddScheme<SimpleTokenAuthenticationOptions, SimpleTokenAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
		}
	}
}
