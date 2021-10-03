using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SkipAuthenticationSchemes.Security;
using SkipAuthenticationSchemes.Services;

namespace SkipAuthenticationSchemes
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddAuthentication()
				.AddScheme<NoResultAuthOptions, NoResultAuthenticationHandler>(NoResultAuthOptions.NoResultAuthenticationScheme, null)
				.AddScheme<BasicAuthenticationSchemeOptions, BasicAuthenticationHandler>(AuthenticationSchemes.BasicAuthentication, null)
				.AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(AuthenticationSchemes.ApiKey, options =>
					{
						Configuration.Bind("App", options);
						options.ForwardDefaultSelector =
							context =>
								context.Request.Headers.ContainsKey("Authorization")
									? NoResultAuthOptions.NoResultAuthenticationScheme
									: null;
					});

			services.AddSingleton<IUserService, UserService>();
			services.AddHttpContextAccessor();

			// add authorization handlers used by BasicAuthenticationOrXApiKeyPolicy
			services.AddSingleton<IAuthorizationHandler, HasApiKeyHandler>();
			services.AddSingleton<IAuthorizationHandler, HasBasicAuthenticationHandler>();

			services.AddAuthorization(options =>
			{
				options.AddAuthorizationPolicies();
				// setting fallback applies this policy to any endpoint that doesn't explicitly define authorization (see Configure:app.UseEndpoints)
				options.FallbackPolicy = AuthorizationPolicies.DefaultAccessPolicy;
			});

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.EnableAnnotations();
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SkipAuthenticationSchemes", Version = "v1" });
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkipAuthenticationSchemes v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				// by omitting authorization here, we're saying that by default nothing that states authorization won't require it;
				// but since we set a FallbackPolicy above, we're actually overriding and setting a default policy
				endpoints.MapControllers();
				// if we were to do the following, this is the same as putting an [Authorize] attribute at the top of every controller
				//endpoints.MapControllers().RequireAuthorization();
			});
		}
	}
}
