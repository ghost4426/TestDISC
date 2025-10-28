using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using TestDISC.Actions;
using TestDISC.Actions.Interfaces;
using TestDISC.Database;
using TestDISC.Models.UtilsProject;
using TestDISC.MServices;
using TestDISC.MServices.Interfaces;
using TestDISC.Queries;
using TestDISC.Queries.Interfaces;
using TestDISC.Services;
using TestDISC.Services.Interfaces;

namespace TestDISC
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
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(Path.Combine("{0}", "TestDISC.xml"), AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TestDISC",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            #endregion

            //Configuration from AppSettings
            services.Configure<JWT>(Configuration.GetSection("JWT"));
            services.Configure<LinkBase>(Configuration.GetSection("LinkBase"));
            services.Configure<ChromeSetting>(Configuration.GetSection("ChromeSetting"));

            //Adding Athentication - JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                });

            //System
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //MServices
            services.AddSingleton<ITestDISCDapper, TestDISCDapper>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISeleniumService, SeleniumService>();

            //Services
            services.AddScoped<IQuestionGroupServices, QuestionGroupServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IInfoTestServices, InfoTestServices>();
            services.AddScoped<IResultDISCServices, ResultDISCServices>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<ILogActionService, LogActionService>();

            //Queries
            services.AddScoped<IQuestionGroupQueries, QuestionGroupQueries>();
            services.AddScoped<IQuestionQueries, QuestionQueries>();
            services.AddScoped<IResultDISCQueries, ResultDISCQueries>();
            services.AddScoped<IUserAnswerQueries, UserAnswerQueries>();
            services.AddScoped<IUserQuery, UserQuery>();
            services.AddScoped<IAuthQuery, AuthQuery>();
            services.AddScoped<IPartnerQuery, PartnerQuery>();

            //Actions
            services.AddScoped<IUserActions, UserActions>();
            services.AddScoped<IResultDISCActions, ResultDISCActions>();
            services.AddScoped<IAuthAction, AuthAction>();
            services.AddScoped<ILogActionAction, LogActionAction>();

            services.AddDbContext<TestDISCContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("TestDISCConnection")), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            services.AddControllersWithViews();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            //Deploy
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            #region Swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestDISC");
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
