
using Microsoft.AspNetCore.Mvc;

using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.API.Controllers
{
    #region BaseController

    /// <summary>
    /// Base Controller
    /// </summary>      
    public class BaseController : ControllerBase
    {
        #region NonAction Methods

        /// <summary>
        /// Create Response
        /// </summary>
        /// <param name="datatable"></param>
        /// <returns></returns>
        //[NonAction]
        //public async Task<IActionResult> CreateResponseAsync(DataTable datatable)
        //{
        //    //SpResponse response = await SPResponse(datatable);

        //    //if (!response.Success)
        //    //{
        //    //    throw new ValidationException(response.Error);
        //    //}

        //    //return new OkObjectResult(Newtonsoft.Json.JsonConvert.DeserializeObject(response.JsonString));
        //}

        /// <summary>
        /// Get deserialize JOSN from data-table
        /// </summary>
        /// <param name="datatable"></param>
        /// <returns></returns>
        //[NonAction]
        //public static async Task<SpResponse> SPResponse(DataTable datatable)
        //{
        //    return await Task.FromResult(datatable.Rows.Cast<DataRow>().Select(row => new SpResponse
        //    {
        //        JsonString = row["JSON_OUTPUT_DATA"] is DBNull ? string.Empty : Convert.ToString(row["JSON_OUTPUT_DATA"]),
        //        Success = row["SUCCESS"] is DBNull ? false : Convert.ToBoolean(row["SUCCESS"]),
        //        Error = row["ERROR_MESSAGE"] is DBNull ? string.Empty : Convert.ToString(row["ERROR_MESSAGE"])
        //    }).FirstOrDefault());
        //}

        #endregion
    }

    #endregion
}