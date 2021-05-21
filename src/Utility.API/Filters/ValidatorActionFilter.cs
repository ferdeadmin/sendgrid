using Utility.dataLayer.Abstract;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Reflection;

namespace Utility.API.Filters
{
    #region ValidatorActionFilter
    /// <summary>
    /// ValidatorActionFilter
    /// </summary>
    public class ValidatorActionFilter : IActionFilter
    {
        #region Properties and Fields

        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region CTOR

        /// <summary>
        /// Validator Action Filter CTOR
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        public ValidatorActionFilter(ILogger<ValidatorActionFilter> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            if (!filterContext.ModelState.IsValid)
            {

                filterContext.Result = new ValidationFailedResult(filterContext.ModelState, _logger, _httpContextAccessor, filterContext.RouteData.Values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
    #endregion

    #region ValidationFailedResult
    /// <summary>
    /// Validation Failed Result
    /// </summary>
    public class ValidationFailedResult : ObjectResult
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="routeValueDictionary"></param>
        public ValidationFailedResult(ModelStateDictionary modelState, ILogger logger, IHttpContextAccessor httpContextAccessor, RouteValueDictionary routeValueDictionary)
            : base(new ValidationResultModel(modelState, logger, httpContextAccessor, routeValueDictionary))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
    #endregion

    #region ValidationError

    /// <summary>
    /// Validation Error
    /// </summary>
    public class ValidationError
    {
        #region Properties and Fields
        /// <summary>
        /// Error Fields
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; }

        #endregion

        #region CTOR

        /// <summary>
        /// CTOR 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="message"></param>
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }

        #endregion
    }
    #endregion

    #region ValidationResultModel
    /// <summary>
    /// Validation Result
    /// </summary>
    public class ValidationResultModel
    {
        #region Properties and Fields

        /// <summary>
        /// Error Message 
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// List of Errors
        /// </summary>
        public List<ValidationError> Errors { get; }

        /// <summary>
        /// Alarm Service 
        /// </summary>
        /// <summary>
        /// Route Data Dictionary Containing all values of Route Parameters.
        /// </summary>
        private readonly IDictionary<string, object> _routeValueDictionary;

        #endregion

        #region CTOR

        /// <summary>
        /// Validation Result Model.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="routeValueDictionary"></param>
        public ValidationResultModel(ModelStateDictionary modelState, ILogger logger, IHttpContextAccessor httpContextAccessor, RouteValueDictionary routeValueDictionary)
        {
            httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISqlHelper>();


            //Route Data 
            _routeValueDictionary = routeValueDictionary;

            Message = "Validation Failed";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            string errorMessage = string.Join(",", Errors.Select(x => x.Field + ": " + x.Message));

            Errors = modelState.Keys
                   .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                   .ToList();

            logger.LogError(errorMessage);
        }

        #endregion

    }

    #endregion

}

