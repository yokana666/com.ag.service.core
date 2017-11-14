using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Com.DanLiris.Service.Core.Lib;
using Microsoft.EntityFrameworkCore;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.WebApi.v1.ViewModels;
using Com.DanLiris.Service.Core.WebApi.Helpers;
using Com.Moonlay.NetCore.Lib.Service;
using Com.Moonlay.NetCore.Lib;
using System.Reflection;
using Newtonsoft.Json;

namespace Com.DanLiris.Service.Core.WebApi.v1.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/master/buyers")]
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
        public IActionResult GetBuyer(int Page = 1, int Size = 25, string Order = "{}", [Bind(Prefix = "Select[]")]List<string> Select = null, string Keyword = null)
        {
            try
            {

                IQueryable<Buyer> Query = _context.Buyers;
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

                /* Search With Keyword */
                if (Keyword != null)
                {
                    List<string> SearchAttributes = new List<string>()
                    {
                        "Name"
                    };

                    Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
                }

                /* Dynamic Select
                    if (Select.Count != 0)
                    {
                        Select.Add("Id");

                        Query = Query
                            .Select<Buyer>(string.Concat("new(", string.Join(",", Select), ")"));   
                    }
                */

                /* Const Select */
                List<string> SelectedFields = new List<string>()
                {
                    "_id", "code", "name", "address", "city", "country", "contact", "tempo"
                };

                Query = Query
                    .Select(b => new Buyer
                    {
                        Id = b.Id,
                        Code = b.Code,
                        Name = b.Name,
                        Address = b.Address,
                        City = b.City,
                        Country = b.Country,
                        Contact = b.Contact,
                        Tempo = b.Tempo
                    });

                /* Order */
                if (OrderDictionary.Count.Equals(0))
                {
                    OrderDictionary.Add("_updatedDate", General.DESCENDING);

                    Query = Query.OrderByDescending(b => b._LastModifiedUtc); /* Default Order */
                }
                else
                {
                    string Key = OrderDictionary.Keys.First();
                    string OrderType = OrderDictionary[Key];
                    string TransformKey = General.TransformOrderBy(Key);

                    BindingFlags IgnoreCase = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                    Query = OrderType.Equals(General.ASCENDING) ?
                        Query.OrderBy(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b)) :
                        Query.OrderByDescending(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b));
                }

                /* Pagination */
                Pageable<Buyer> pageable = new Pageable<Buyer>(Query, Page - 1, Size);
                List<Buyer> Data = pageable.Data.ToList<Buyer>();

                int TotalData = pageable.TotalCount;
                int TotalPageData = Data.Count();

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<Buyer, BuyerViewModel>(Data, MapToViewModel, Page, Size, TotalData, TotalPageData, OrderDictionary, SelectedFields);

                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
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
                    new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                    .Fail();
                return NotFound(Result);
            }

            try
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<Buyer, BuyerViewModel>(buyer, MapToViewModel);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuyer([FromRoute] int id, [FromBody] BuyerViewModel buyerVM)
        {
            Buyer buyer = MapToModel(buyerVM);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buyer.Id)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
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
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!BuyerExists(id))
                {
                    Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                    .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                        .Fail(e);
                    return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostBuyer([FromBody] BuyerViewModel buyerVM)
        {
            Buyer buyer = MapToModel(buyerVM);

            /* Generate Code
                CodeGenerator CGenerator = new CodeGenerator();
                var CodeExists = false;

                do
                {
                    buyer.Code = CGenerator.GenerateCode();
                }
                while (_context.Buyers.Any(b => b.Code.Equals(buyer.Code)););
            */

            try
            {
                await _service.CreateAsync(buyer);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(HttpContext.Request.Path, "/", buyer.Id), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
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
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        private bool BuyerExists(int id)
        {
            return _context.Buyers.Any(e => e.Id.Equals(id));
        }

        private BuyerViewModel MapToViewModel(Buyer buyer)
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

        private Buyer MapToModel(BuyerViewModel buyerVM)
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
