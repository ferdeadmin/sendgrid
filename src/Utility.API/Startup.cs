using Utility.API.Filters;

using DomainModel;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        #endregion


        #region Properties
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Configure Services
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationConfigurations>(this.Configuration.GetSection("ConnectionStrings"));
            services.Configure<ConnectionTimeOut>(this.Configuration.GetSection("ConnectionTimeOut"));
            

            string test = this.Configuration["ConnectionTimeOut:TimeOut"];

     

            //Swagger Configuration and Add Swagger generation Document       
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Utility V1.0", Version = "v1" });

                // Add JWT token in header.
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description =
                //            "JWT Authorization header using the Bearer scheme." +
                //            "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            },
                //            Scheme = "oauth2",
                //            Name = "Bearer",
                //            In = ParameterLocation.Header,

                //        },
                //        new List<string>()
                //    }
                //});

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                //c.OperationFilter<FileUploadOperation>();
            });

            services.AddControllersWithViews(opt =>
            {
                opt.Filters.Add(typeof(ValidatorActionFilter));
                opt.Filters.Add(typeof(CustomExceptionFilter));
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            //services.AddResponseCompression(options =>
            //{
            //    options.EnableForHttps = true;
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(RoleConstants.Admin, policy => policy.RequireRole(RoleConstants.Admin));
            //    options.AddPolicy(RoleConstants.Accounting, policy => policy.RequireRole(RoleConstants.Accounting));
            //    options.AddPolicy(RoleConstants.SeniorCaseofficer, policy => policy.RequireRole(RoleConstants.SeniorCaseofficer));
            //    options.AddPolicy(RoleConstants.Caseofficer, policy => policy.RequireRole(RoleConstants.Caseofficer));
            //    options.AddPolicy(RoleConstants.SuperAdmin, policy => policy.RequireRole(RoleConstants.SuperAdmin));
            //});

            //// Needed for jwt auth.
            //services
            //    .AddAuthentication(options =>
            //    {
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(cfg =>
            //    {
            //        cfg.RequireHttpsMetadata = false;
            //        cfg.SaveToken = true;
            //        cfg.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidIssuer = this.Configuration["BearerTokensOptions:Issuer"], // site that makes the token
            //            ValidateIssuer = false, // TODO: change this to avoid forwarding attacks
            //            ValidAudience = this.Configuration["BearerTokensOptions:Audience"], // site that consumes the token
            //            ValidateAudience = false, // TODO: change this to avoid forwarding attacks
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["BearerTokensOptions:Key"])),
            //            ValidateIssuerSigningKey = true, // verify signature to avoid tampering
            //            ValidateLifetime = true, // validate the expiration
            //            ClockSkew = TimeSpan.Zero // tolerance for the expiration date
            //        };
            //        cfg.Events = new JwtBearerEvents
            //        {
            //            OnAuthenticationFailed = context =>
            //            {
            //                Microsoft.Extensions.Logging.ILogger logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
            //                logger.LogError("Authentication failed.", context.Exception);
            //                return Task.CompletedTask;
            //            },
            //            OnTokenValidated = context =>
            //            {

            //                ITokenValidatorService tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
            //                return tokenValidatorService.ValidateAsync(context);
            //            },
            //            OnMessageReceived = context =>
            //            {
            //                return Task.CompletedTask;
            //            },
            //            OnChallenge = context =>
            //            {
            //                Microsoft.Extensions.Logging.ILogger logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
            //                logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);
            //                return Task.CompletedTask;
            //            }
            //        };
            //    });

            services.AddDataProtection()
                   .UseCryptographicAlgorithms(
                   new AuthenticatedEncryptorConfiguration()
                   {
                       EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                       ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                   })
                   .SetDefaultKeyLifetime(TimeSpan.FromDays(7));

            //services.Configure<FTPDetails>(this.Configuration.GetSection("FTPDetails"));

            // Config IOC
            IOCConfig.Register(services);
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseResponseCompression();

            app.UseStaticFiles();

            app.UseSwagger()
              .UseDefaultFiles()
              .UseStaticFiles()
              // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Utility API");
              });

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    IExceptionHandlerFeature error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var response = new { message = "token expired", statusCode = System.Net.HttpStatusCode.Unauthorized };
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        var response = new { message = error.Error.Message, statusCode = System.Net.HttpStatusCode.InternalServerError };
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        await next();
                    }
                });
            });
            app.UseStatusCodePages();
            app.UseRouting();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        #endregion
    }
}
