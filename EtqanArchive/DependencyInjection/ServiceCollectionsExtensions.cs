using EtqanArchive.BackEnd.Services;
using EtqanArchive.DataLayer;
using EtqanArchive.Localization.DependnecyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EtqanArchive.BackEnd.DependencyInjection
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddDBContext(this IServiceCollection collection, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("Configuration cannot be null");

            // Register Base Sql Db Context
            collection.AddDbContext<EtqanArchiveDBContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return collection;
        }

        public static IServiceCollection AddSecurityServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }


        public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        {
            services.AddDefaultLocalization(EtqanArchive.Localization.Enums.LocalizationProvider.Header);
            return services;
        }


    }
}
