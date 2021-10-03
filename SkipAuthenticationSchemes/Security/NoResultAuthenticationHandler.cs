using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SkipAuthenticationSchemes.Security
{
	public class NoResultAuthOptions : AuthenticationSchemeOptions
	{
		public const string NoResultAuthenticationScheme = nameof(NoResultAuthenticationScheme);
	}

	public class NoResultAuthenticationHandler : AuthenticationHandler<NoResultAuthOptions>
	{
		public NoResultAuthenticationHandler(IOptionsMonitor<NoResultAuthOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock) : base(options, logger, encoder, clock)
		{
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			return Task.FromResult(AuthenticateResult.NoResult());
		}
	}
}