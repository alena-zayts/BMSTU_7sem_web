using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Options;
using WebApplication1.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApplication1
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

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new PatchRequestContractResolver(); // patch
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SkiResort",
                    Description = "API for Web Course Project",
                    Version = "v1",
                });

                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                options.IncludeXmlComments(filePath);


                //
                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                //    "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                //    "Example: \"Bearer 1safsfsdfdfd\"",
                //});
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                // {
                //     {
                //           new OpenApiSecurityScheme
                //             {
                //                 Reference = new OpenApiReference
                //                 {
                //                     Type = ReferenceType.SecurityScheme,
                //                     Id = "Bearer"
                //                 }
                //             },
                //             new string[] {}
                //     }
                // });

                //swaggerB
                options.DocumentFilter<SwaggerIgnoreFilter>(); 

            });

            //

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        //options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // ��������, ����� �� �������������� �������� ��� ��������� ������
                            ValidateIssuer = true,
                            // ������, �������������� ��������
                            ValidIssuer = AuthOptions.ISSUER,

                            // ����� �� �������������� ����������� ������
                            ValidateAudience = true,
                            // ��������� ����������� ������
                            ValidAudience = AuthOptions.AUDIENCE,
                            // ����� �� �������������� ����� �������������
                            ValidateLifetime = true,

                            // ��������� ����� ������������
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // ��������� ����� ������������
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
            

            //swaggerB
            //https://stackoverflow.com/questions/38184583/how-to-add-ihttpcontextaccessor-in-the-startup-class-in-the-di-in-asp-net-core-1
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseDefaultFiles(); //JWT 
            app.UseStaticFiles(); //JWT

            app.UseAuthentication(); //JWT
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //This is authentification in swagger 1
            //app.UseMiddleware<SwaggerUrlProtectorMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiResort v1");
                c.DefaultModelsExpandDepth(-1);
                //Here goes your oAuth setup
                //This is authentification in swagger 2 (AB)
                c.UseResponseInterceptor("function (res) {if (res.ok && res.body){if (res.body.access_token) {sessionStorage.setItem('authKey', res.body.access_token);location.reload();}if (res.body.delete_access_token) {sessionStorage.removeItem('authKey');location.reload();}}return res;}");
                c.UseRequestInterceptor("function (req) {var key = sessionStorage.getItem('authKey');if (key && key.trim() !== ''){req.headers.Authorization = 'Bearer ' + key;} return req;}");
                

            });

        }
    }
}
