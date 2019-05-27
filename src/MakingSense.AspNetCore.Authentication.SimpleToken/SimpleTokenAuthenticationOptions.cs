using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Framework.Internal;
using Microsoft.IdentityModel.Tokens;

namespace MakingSense.AspNetCore.Authentication.SimpleToken
{
	/// <summary>
	/// Options class provides information needed to control SimpleToken middleware behavior
	/// </summary>
	public class SimpleTokenAuthenticationOptions : AuthenticationSchemeOptions
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

		public override void Validate()
		{
			base.Validate();

			if (SecurityTokenValidatorsFactory == null)
			{
				throw new ArgumentException(nameof(SecurityTokenValidatorsFactory));
			}

			if (TokenValidationParameters == null)
			{
				throw new ArgumentException(nameof(TokenValidationParameters));
			}
		}
	}
}
