using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using EtqanArchive.Localization.Enums;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace EtqanArchive.Localization.DependnecyInjection
{
    public static class LocalizationServiceCollectionExtensions
    {
        /// <summary>
        /// Add Localization based on the provider supplied, it can be from Header or from Query
        /// </summary>
        /// <param name="services"></param>
        /// <param name="provider"> the provider type , Header or query string</param>
        /// <returns></returns>

        private const string QueryStringKey = "locale";
        public static IServiceCollection AddDefaultLocalization(this IServiceCollection services, LocalizationProvider provider)
        {
            //services.AddLocalization(
            ////    opt =>
            ////{
            ////    opt.ResourcesPath = "Resources";
            ////}
            //);

            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });
            var options = BuildRequestLocalizationOptions();
            IRequestCultureProvider cultureProvider;
            cultureProvider = provider == LocalizationProvider.Header ?
             GetHeaderProvider() : GetQueryStringProvider();
            options.RequestCultureProviders.Insert(0, cultureProvider);
            services.Configure<RequestLocalizationOptions>(opt =>
            {
                opt.DefaultRequestCulture = options.DefaultRequestCulture;
                opt.SupportedCultures = options.SupportedCultures;
                opt.SupportedUICultures = options.SupportedUICultures;
                opt.RequestCultureProviders = options.RequestCultureProviders;
            });
            return services;
        }

        #region Helpers
        private static RequestLocalizationOptions BuildRequestLocalizationOptions()
        {
            var options = new RequestLocalizationOptions();
            var supportedCultures = new List<CultureInfo>
               {
                new CultureInfo("en"),
                new CultureInfo("ar")
               };
            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            //var cultureInfo = supportedCultures.FirstOrDefault(a => a.Name == "en");
            //cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
            return options;
        }

        private static IRequestCultureProvider GetQueryStringProvider()
        {
            var provider = new CustomRequestCultureProvider(context =>
            {
                string userLang = string.Empty;
                if (!string.IsNullOrWhiteSpace(QueryStringKey))
                {
                    userLang = context.Request.Query[QueryStringKey];
                    if (userLang == "ar_EG")
                        userLang = "ar";

                }
                var defaultLang = string.IsNullOrEmpty(userLang) ? "ar" : userLang;
                return Task.FromResult(new ProviderCultureResult(defaultLang, defaultLang));
            });

            return provider;
        }
        private static IRequestCultureProvider GetHeaderProvider()
        {
            var provider = new CustomRequestCultureProvider(context =>
            {
                var userLangs = context.Request.Headers["Accept-Language"].ToString();
                var firstLang = userLangs.Split(',').FirstOrDefault();
                var defaultLang = string.IsNullOrEmpty(firstLang) ? "ar" : firstLang;

                return Task.FromResult(new ProviderCultureResult(defaultLang, defaultLang));
            });

            return provider;
        }

        #endregion
        //public static string GetGlobalResourceByPreferredLang(this IStringLocalizer<GlobalStringsResources> Resource, int PreferredLang)
        //{
        //    var culture = PreferredLang == 1 ? new CultureInfo("en-US") : new CultureInfo("ar-SA");
        //    ResourceManagerStringLocalizer resource = new ResourceManagerStringLocalizer(new System.Resources.ResourceManager())
        // //   return GlobalStrings.ResourceManager.GetString(Resource, culture);
        //}

        //public static string GetGlobalMessagesByPreferredLang(this IStringLocalizer<ValidationResources> Resource, int PreferredLang)
        //{
        //    var culture = PreferredLang == 1 ? new CultureInfo("en-US") : new CultureInfo("ar-SA");
        //    //return GlobalMessages.ResourceManager.GetString(Resource, culture);
        //}

    }
}
