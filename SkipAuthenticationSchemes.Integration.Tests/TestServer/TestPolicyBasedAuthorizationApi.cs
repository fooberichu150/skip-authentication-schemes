namespace SkipAuthenticationSchemes.Integration.Tests.TestServer
{
	public sealed class TestSkipAuthenticationSchemesApi
    {
        public TestSkipAuthenticationSchemesApiFactory Api { get; }

        public static TestSkipAuthenticationSchemesApi Instance { get; } = new TestSkipAuthenticationSchemesApi();

        static TestSkipAuthenticationSchemesApi() { }
        private TestSkipAuthenticationSchemesApi()
        {
            Api = new TestSkipAuthenticationSchemesApiFactory();
        }
    }
}
