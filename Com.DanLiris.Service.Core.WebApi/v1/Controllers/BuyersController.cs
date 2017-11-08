using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.DanLiris.Service.Core.Lib;
using Microsoft.EntityFrameworkCore;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.WebApi.v1.ViewModels;
using Com.DanLiris.Service.Core.WebApi.Helpers;
using Com.Moonlay.NetCore.Lib.Service;
using Com.Moonlay.NetCore.Lib;

namespace Com.DanLiris.Service.Core.WebApi.v1.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/buyers")]
    public class BuyersController : Controller
    {
        private readonly CoreDbContext _context;
        private readonly BuyerService _service;
        private readonly string ApiVersion = "1.0";

        public BuyersController(CoreDbContext context, BuyerService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetBuyer(int Page = 1, int Size = 25, Dictionary<string, object> Order = null)
        {
            try
            {
                int TotalData = _context.Buyers.Count();

                List<Buyer> Data = new Pageable<Buyer>(_context.Buyers, Page - 1, Size).Data.ToList<Buyer>();
                int TotalPageData = Data.Count();

                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.OK_STATUS_CODE, Common.OK_MESSAGE)
                    .Ok(Data, ViewModelMap, Page, Size, TotalData, TotalPageData);
                
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuyer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buyer = await _context.Buyers.SingleOrDefaultAsync(m => m.Id == id);

            if (buyer == null)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.NOT_FOUND_STATUS_CODE, Common.NOT_FOUND_MESSAGE)
                    .Fail();
                return NotFound(Result);
            }

            try
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.OK_STATUS_CODE, Common.OK_MESSAGE)
                    .Ok(buyer, ViewModelMap);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuyer([FromRoute] int id, [FromBody] BuyerViewModel buyerVM)
        {
            Buyer buyer = ModelMap(buyerVM);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buyer.Id)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.BAD_REQUEST_STATUS_CODE, Common.BAD_REQUEST_MESSAGE)
                    .Fail();
                return BadRequest(Result);
            }

            try
            {
                await _service.UpdateAsync(id, buyer);

                return NoContent();
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.BAD_REQUEST_STATUS_CODE, Common.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!BuyerExists(id))
                {
                    Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.NOT_FOUND_STATUS_CODE, Common.NOT_FOUND_MESSAGE)
                    .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                        .Fail(e);
                    return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostBuyer([FromBody] BuyerViewModel buyerVM)
        {
            Buyer buyer = ModelMap(buyerVM);

            try
            {
                await _service.CreateAsync(buyer);

                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.CREATED_STATUS_CODE, Common.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(HttpContext.Request.Path, "/", buyer.Id), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.BAD_REQUEST_STATUS_CODE, Common.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuyer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Buyer, BuyerViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        private bool BuyerExists(int id)
        {
            return _context.Buyers.Any(e => e.Id == id);
        }

        private BuyerViewModel ViewModelMap(Buyer buyer)
        {
            BuyerViewModel buyerVM = new BuyerViewModel();

            buyerVM._id = buyer.Id;
            buyerVM._deleted = buyer._IsDeleted;
            buyerVM._active = buyer.Active;
            buyerVM._createdDate = buyer._CreatedUtc;
            buyerVM._createdBy = buyer._CreatedBy;
            buyerVM._createAgent = buyer._CreatedAgent;
            buyerVM._updatedDate = buyer._LastModifiedUtc;
            buyerVM._updatedBy = buyer._LastModifiedBy;
            buyerVM._updateAgent = buyer._LastModifiedAgent;
            buyerVM.code = buyer.Code;
            buyerVM.name = buyer.Name;
            buyerVM.address = buyer.Address;
            buyerVM.city = buyer.City;
            buyerVM.country = buyer.Country;
            buyerVM.contact = buyer.Contact;
            buyerVM.tempo = buyer.Tempo;
            buyerVM.type = buyer.Type;
            buyerVM.NPWP = buyer.NPWP;

            return buyerVM;
        }

        private Buyer ModelMap(BuyerViewModel buyerVM)
        {
            Buyer buyer = new Buyer();

            buyer.Id = buyerVM._id;
            buyer._IsDeleted = buyerVM._deleted;
            buyer.Active = buyerVM._active;
            buyer._CreatedUtc = buyerVM._createdDate;
            buyer._CreatedBy = buyerVM._createdBy;
            buyer._CreatedAgent = buyerVM._createAgent;
            buyer._LastModifiedUtc = buyerVM._updatedDate;
            buyer._LastModifiedBy = buyerVM._updatedBy;
            buyer._LastModifiedAgent = buyerVM._updateAgent;
            buyer.Code = buyerVM.code;
            buyer.Name = buyerVM.name;
            buyer.Address = buyerVM.address;
            buyer.City = buyerVM.city;
            buyer.Country = buyerVM.country;
            buyer.Contact = buyerVM.contact;
            buyer.Tempo = buyerVM.tempo;
            buyer.Type = buyerVM.type;
            buyer.NPWP = buyerVM.NPWP;

            return buyer;
        }
    }
}
