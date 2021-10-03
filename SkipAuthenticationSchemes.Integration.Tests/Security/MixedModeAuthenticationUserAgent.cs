using System.Net.Http;
using SkipAuthenticationSchemes.Integration.Tests.Constants;
using SkipAuthenticationSchemes.Integration.Tests.TestServer;

namespace SkipAuthenticationSchemes.Integration.Tests.Security
{
	public class MixedModeAuthenticationUserAgent : IUserAgent
	{
		public MixedModeAuthenticationUserAgent(TestSkipAuthenticationSchemesApiFactory apiFactory,
			string userName,
			string password,
			string headerKey,
			string apiKey = null)
		{
			var textBytes = System.Text.Encoding.UTF8.GetBytes($"{userName}:{password}");
			var encoded = System.Convert.ToBase64String(textBytes);

			Client = apiFactory.CreateClient();
			Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);
			Client.DefaultRequestHeaders.Add(headerKey, apiKey ?? TestAuthorization.ApiKey);
		}

		public HttpClient Client { get; }

		public void Dispose()
		{
			Client.Dispose();
		}
	}
}