using System.Net.Http;
using SkipAuthenticationSchemes.Integration.Tests.TestServer;
using SkipAuthenticationSchemes.Integration.Tests.Constants;

namespace SkipAuthenticationSchemes.Integration.Tests.Security
{
	public class ApiKeyUserAgent : IUserAgent
	{
        public ApiKeyUserAgent(TestSkipAuthenticationSchemesApiFactory apiFactory,
            string headerKey,
            string apiKey = null)
        {
            Client = apiFactory.CreateClient();
            Client.DefaultRequestHeaders.Add(headerKey, apiKey ?? TestAuthorization.ApiKey);
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
