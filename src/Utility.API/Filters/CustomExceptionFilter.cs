using Utility.dataLayer.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility.API.Filters
{
    #region CustomExceptionFilter
    /// <summary>
    /// Custom Exception Filter
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        #region Properties and Fields

        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="valuesForAppScope"></param>
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger, IHttpContextAccessor httpContextAccessor )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        # region Methods
        /// <summary>
        /// On Exception
        /// </summary>
        /// <param name="context"></param>
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISqlHelper>();
    

           

        }
        #endregion
    }
    #endregion
}
