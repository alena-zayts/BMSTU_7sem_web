using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.SpaServices;
using AutoMapper;

namespace WebApplication2
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
			//services.AddMemoryCache();
			//services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			//services.AddReact();
			//services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();

			services.AddMvc();
			//services.AddAutoMapper();
		}

		public void Configure(IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebpackDevMiddleware(); // 1
			}

			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "DefaultApi",
					template: "api/{controller}/{action}");
				routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" }); // 2
			});
		}
	}
}