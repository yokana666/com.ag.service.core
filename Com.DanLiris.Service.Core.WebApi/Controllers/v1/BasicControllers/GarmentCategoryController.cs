using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/master/garment-categories")]
    public class GarmentCategoryController : BasicController<GarmentCategoryService, GarmentCategory, GarmentCategoryViewModel, CoreDbContext>
    {
        private static readonly string ApiVersion = "1.0";
        GarmentCategoryService service;
        public GarmentCategoryController(GarmentCategoryService service) : base(service, ApiVersion)
        {
        }

        [HttpGet("byCode/{codeRequirement}")]
        public IActionResult GetByCode([FromRoute] string codeRequirement)
        {
            try
            {
                service.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                List<GarmentCategory> Data = service.GetByCode(codeRequirement);

                Dictionary<string, object> Result =
                     new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                     .Ok(Data);

                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}