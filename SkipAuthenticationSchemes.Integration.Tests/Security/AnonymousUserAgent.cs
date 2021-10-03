using System.Net.Http;
using SkipAuthenticationSchemes.Integration.Tests.TestServer;

namespace SkipAuthenticationSchemes.Integration.Tests.Security
{
	public class AnonymousUserAgent : IUserAgent
	{
        public AnonymousUserAgent(TestSkipAuthenticationSchemesApiFactory apiFactory)
        {
            Client = apiFactory.CreateClient();
        }

        public HttpClient Client { get; }
    }
}
