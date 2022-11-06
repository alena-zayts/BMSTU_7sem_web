using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProjectReactRedux.Options;



namespace ProjectReactRedux
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();








            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            //options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // укзывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = AuthOptions.ISSUER,

                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = AuthOptions.AUDIENCE,
                // будет ли валидироваться время существования
                ValidateLifetime = true,

                // установка ключа безопасности
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true,
            };
        });













            services.AddSingleton<AccessToDB.TarantoolContext>();
            services.AddTransient<BL.IRepositoriesFactory, AccessToDB.TarantoolRepositoriesFactory>();

            services.AddTransient<BL.IRepositories.IUsersRepository, AccessToDB.RepositoriesTarantool.TarantoolUsersRepository>();
            services.AddTransient<BL.IRepositories.IMessagesRepository, AccessToDB.RepositoriesTarantool.TarantoolMessagesRepository>();
            services.AddTransient<BL.IRepositories.ILiftsRepository, AccessToDB.RepositoriesTarantool.TarantoolLiftsRepository>();
            services.AddTransient<BL.IRepositories.ISlopesRepository, AccessToDB.RepositoriesTarantool.TarantoolSlopesRepository>();
            services.AddTransient<BL.IRepositories.ILiftsSlopesRepository, AccessToDB.RepositoriesTarantool.TarantoolLiftsSlopesRepository>();
            services.AddTransient<BL.IRepositories.ITurnstilesRepository, AccessToDB.RepositoriesTarantool.TarantoolTurnstilesRepository>();
            services.AddTransient<BL.IRepositories.ICardsRepository, AccessToDB.RepositoriesTarantool.TarantoolCardsRepository>();
            services.AddTransient<BL.IRepositories.ICardReadingsRepository, AccessToDB.RepositoriesTarantool.TarantoolCardReadingsRepository>();

            //services.AddTransient<BL.Facade>();
            services.AddTransient<BL.Services.CardsService>();
            services.AddTransient<BL.Services.SlopesService>();
            services.AddTransient<BL.Services.LiftsService>();
            services.AddTransient<BL.Services.LiftsSlopesService>();
            services.AddTransient<BL.Services.TurnstilesService>();
            services.AddTransient<BL.Services.UsersService>();
            services.AddTransient<BL.Services.MessagesService>();
            services.AddTransient<BL.Services.CardReadingsService>();






            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
                //configuration.RootPath = "NewApp/my-app/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();




            app.UseDefaultFiles(); //JWT 
            app.UseStaticFiles(); //JWT
            app.UseAuthentication(); //JWT
            app.UseAuthorization();





            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                //spa.Options.SourcePath = "NewApp/my-app";
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
