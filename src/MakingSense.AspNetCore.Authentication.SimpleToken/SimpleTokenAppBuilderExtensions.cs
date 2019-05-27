using System;
using MakingSense.AspNetCore.Authentication.SimpleToken;

namespace Microsoft.AspNetCore.Builder
{
	public static class SimpleTokenAppBuilderExtensions
	{
		[Obsolete("UseSimpleTokenAuthentication is obsolete. Configure SimpleTokenAuthentication authentication with AddAuthentication().AddSimpleTokenAuthentication in ConfigureServices. See https://go.microsoft.com/fwlink/?linkid=845470 for more details.", error: true)]
		public static IApplicationBuilder UseSimpleTokenAuthentication(this IApplicationBuilder app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			return UseSimpleTokenAuthentication(app, new SimpleTokenAuthenticationOptions());
		}

		[Obsolete("UseSimpleTokenAuthentication is obsolete. Configure SimpleTokenAuthentication authentication with AddAuthentication().AddSimpleTokenAuthentication in ConfigureServices. See https://go.microsoft.com/fwlink/?linkid=845470 for more details.", error: true)]
		public static IApplicationBuilder UseSimpleTokenAuthentication(this IApplicationBuilder app, SimpleTokenAuthenticationOptions options)
		{
			throw new NotSupportedException("This method is no longer supported, see https://go.microsoft.com/fwlink/?linkid=845470");
		}
	}
}
