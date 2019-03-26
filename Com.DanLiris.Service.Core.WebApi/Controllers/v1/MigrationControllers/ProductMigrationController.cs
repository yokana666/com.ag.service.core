using Com.DanLiris.Service.Core.Lib.IntegrationService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.WebApi.Controllers.v1.MigrationControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/migration/products")]
    public class ProductMigrationController : Controller
    {
        private readonly IProductIntegrationService _service;

        public ProductMigrationController(IProductIntegrationService service)
        {
            _service = service;
        }

        [HttpGet("integrate")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _service.IntegrateData();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
