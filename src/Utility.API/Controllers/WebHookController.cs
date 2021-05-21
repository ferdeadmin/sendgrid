using Utility.dataLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

using System.IO;
using System.Threading.Tasks;
using DomainModel;

namespace Utility.API.Controllers
{

    #region AutosysController
    /// <summary>
    /// Autosys Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class WebHookController : BaseController
    {
        #region Properties and Fields
        /// <summary>
        /// Sql Helper
        /// </summary>
        private ISqlHelper _sqlHelper { get; set; }
        private readonly ILogger<WebHookController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;



        #endregion

        #region CTOR

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public WebHookController(IHttpContextAccessor httpContextAccessor,
              ILogger<WebHookController> logger)
        {
            _logger = logger;
            //_sqlHelper = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISqlHelper>();
            _httpContextAccessor = httpContextAccessor;

        }
        #endregion

        #region Actions and Methods



   
        [HttpPost("Utility")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 422)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 501)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 408)]
        public async Task<IActionResult> Webhook([FromBody] RequestViewModel model)
        {
            string jsonstring=Newtonsoft.Json.JsonConvert.SerializeObject(model);
            _logger.LogInformation(jsonstring);

            return Ok();
           
        }
        #endregion
    }
    #endregion
}