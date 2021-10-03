﻿using System.Net.Http;
using SkipAuthenticationSchemes.Integration.Tests.TestServer;

namespace SkipAuthenticationSchemes.Integration.Tests.Security
{
	public class BasicAuthenticationUserAgent : IUserAgent
	{
        public BasicAuthenticationUserAgent(TestSkipAuthenticationSchemesApiFactory apiFactory, string userName, string password)
        {
            var textBytes = System.Text.Encoding.UTF8.GetBytes($"{userName}:{password}");
            var encoded = System.Convert.ToBase64String(textBytes);

            Client = apiFactory.CreateClient();
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}