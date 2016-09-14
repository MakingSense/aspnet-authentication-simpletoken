using System;
using System.Collections.Generic;
using Microsoft.Framework.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.AspNetCore.Builder
{
	/// <summary>
	/// Options class provides information needed to control SimpleToken middleware behavior
	/// </summary>
	public class SimpleTokenAuthenticationOptions : AuthenticationOptions
	{
		/// <summary>
		/// Gets or sets the <see cref="SecurityTokenValidatorsFactory"/> for creating validators for validating tokens.
		/// </summary>
		/// <exception cref="ArgumentNullException">if 'value' is null.</exception>
		public Func<IEnumerable<ISecurityTokenValidator>> SecurityTokenValidatorsFactory { get; [param: NotNull] set; }

		/// <summary>
		/// Gets or sets the TokenValidationParameters
		/// </summary>
		/// <remarks>Contains the types and definitions required for validating a token.</remarks>
		/// <exception cref="ArgumentNullException">if 'value' is null.</exception>
		public TokenValidationParameters TokenValidationParameters { get; [param: NotNull] set; } = new TokenValidationParameters();

		/// <summary>
		/// Creates an instance of SimpleToken authentication options with default values.
		/// </summary>
		public SimpleTokenAuthenticationOptions() : base()
		{
		}

	}
}
