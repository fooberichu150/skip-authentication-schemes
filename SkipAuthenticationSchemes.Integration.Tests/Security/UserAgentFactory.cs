using System;
using SkipAuthenticationSchemes.Integration.Tests.TestServer;
using SkipAuthenticationSchemes.Security;
using SkipAuthenticationSchemes.Integration.Tests.Constants;

namespace SkipAuthenticationSchemes.Integration.Tests.Security
{
	public static class UserAgentFactory
	{
		private const bool AdminRequested = true;
		private const bool AdminNotRequested = false;

		public static IUserAgent CreateUserAgent(AuthenticationType type)
			=> CreateUserAgent(type, false);

		public static IUserAgent CreateApiKeyAgent(AuthenticationType type, string apiKey = null)
		{
			return (type) switch
			{
				AuthenticationType.ServerKey => new ApiKeyUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, HttpHeaderKeys.XServerKey, apiKey),
				AuthenticationType.XApiKey => new ApiKeyUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, HttpHeaderKeys.XApiKey, apiKey),
				_ => throw new NotImplementedException()
			};
		}

		public static IUserAgent CreateUserAgent(AuthenticationType type, bool isAdmin = false)
		{
			return (type, isAdmin) switch
			{
				(AuthenticationType.BasicAuthentication, AdminNotRequested) => new BasicAuthenticationUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, TestAuthorization.StandardUser.UserName, TestAuthorization.StandardUser.Password),
				(AuthenticationType.BasicAuthentication, AdminRequested) => new BasicAuthenticationUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, TestAuthorization.AdminUser.UserName, TestAuthorization.AdminUser.Password),
				(AuthenticationType.ServerKey, _) => new ApiKeyUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, HttpHeaderKeys.XServerKey),
				(AuthenticationType.InvalidServerKey, _) => new ApiKeyUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, HttpHeaderKeys.XServerKey, TestAuthorization.BadApiKey),
				(AuthenticationType.XApiKey, _) => new ApiKeyUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, HttpHeaderKeys.XApiKey),
				(AuthenticationType.InvalidXApiKey, _) => new ApiKeyUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, HttpHeaderKeys.XApiKey, TestAuthorization.BadApiKey),
				(AuthenticationType.Anonymous, _) => new AnonymousUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api),
				(AuthenticationType.MixedMode, AdminNotRequested) => new MixedModeAuthenticationUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, TestAuthorization.StandardUser.UserName, TestAuthorization.StandardUser.Password, HttpHeaderKeys.XServerKey),
				(AuthenticationType.MixedMode, AdminRequested) => new MixedModeAuthenticationUserAgent(TestSkipAuthenticationSchemesApi.Instance.Api, TestAuthorization.AdminUser.UserName, TestAuthorization.AdminUser.Password, HttpHeaderKeys.XServerKey),
				_ => throw new NotImplementedException()
			};
		}
	}
}
