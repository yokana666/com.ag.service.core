using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.DanLiris.Service.Core.Lib;
using Microsoft.EntityFrameworkCore;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Com.DanLiris.Service.Core.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/values")]
    [Authorize]
    public class ValuesController : Controller
    {
        public ValuesController()
        {
        }

        [HttpGet]
        [Authorize("service.core.read")]
        public IEnumerable<string> Get()
        {
            yield return "hello";
        }
    }
}
