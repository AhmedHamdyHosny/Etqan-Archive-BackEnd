using DataLayer.Security.TableEntity;
using EtqanArchive.BackEnd.DependencyInjection;
using EtqanArchive.DataLayer;
using GenericBackEndCore.Classes.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Text;
using EtqanArchive.BackEnd.Services;

namespace EtqanArchive
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

            services.AddAutoMapper((_serviceProvider, cfg) =>
            {
                cfg.CreateMap(typeof(JsonResponse<>), typeof(JsonResponse<>));

            }, typeof(Startup));

            #region Add Services
            services
                .AddDBContext(Configuration)
                .AddTransient<IProjectFileService, ProjectFileService>()
                .AddTransient<IContentTypeService, ContentTypeService>()
                .AddSecurityServices();

            //services.AddScoped<IUserService, UserService>();
            #endregion

            services.AddIdentity<User, Role>(options =>
            {
                // Configure validation logic for passwords
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                // Configure user lockout defaults
                //options.SignIn.RequireConfirmedEmail = true;

                // Lockout settings
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                //options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<EtqanArchiveDBContext>()
            .AddDefaultTokenProviders();


            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    ValidAudience = Configuration["AuthSettings:Audince"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
                    ValidateIssuerSigningKey = true
                };
            });

            //services.AddControllers()
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // add here your route constraint   
            services.Configure<Microsoft.AspNetCore.Routing.RouteOptions>(routeOptions =>
            {
                routeOptions.ConstraintMap.Add("modelIdType", typeof(GenericBackEndCore.Classes.Common.TModelId));
            });


            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EtqanArchiveDBContext etqanArchiveDBContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseClaims();
            //app.AddValidatorOptions();

            etqanArchiveDBContext.Database.Migrate();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
