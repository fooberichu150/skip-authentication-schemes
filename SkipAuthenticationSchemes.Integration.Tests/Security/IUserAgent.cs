using System.Net.Http;

namespace SkipAuthenticationSchemes.Integration.Tests.Security
{
    public interface IUserAgent
    {
        public HttpClient Client { get; }
    }
}
