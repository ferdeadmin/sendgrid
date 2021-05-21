
using Utility.dataLayer.Abstract;
using Utility.dataLayer.Service;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Utility.API
{
    /// <summary>
    /// IOC configuration 
    /// </summary>
    public static class IOCConfig
    {
        #region Register Service

        /// <summary>
        /// Register IOC configuration 
        /// </summary>
        public static void Register(IServiceCollection services)
        {
            //ValuesForAppScope.Security = "secure";

            // Add Services
            //services.AddTransient<ISqlHelper, SqlHelper>();
   
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IODBCHelper, ODBCHelper>();


            

            var serviceProvider = services.BuildServiceProvider();
        }

        #endregion
    }
}
