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

namespace Com.DanLiris.Service.Core.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/buyers")]
    public class BuyersController : Controller
    {
        private readonly CoreDbContext _context;
        private readonly BuyerService _service;

        public BuyersController(CoreDbContext context, BuyerService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Buyer> GetBuyer()
        {
            return _context.Buyers;
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
                return NotFound();
            }

            return Ok(buyer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuyer([FromRoute] int id, [FromBody] Buyer buyer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buyer.Id)
            {
                return BadRequest();
            }

            _context.Entry(buyer).State = EntityState.Modified;
            var nowUtc = DateTime.UtcNow;
            buyer._LastModifiedUtc = nowUtc;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostBuyer([FromBody] Buyer buyer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nowUtc = DateTime.UtcNow;
            buyer._CreatedUtc = buyer._LastModifiedUtc = nowUtc;

            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuyer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buyer = await _context.Buyers.SingleOrDefaultAsync(m => m.Id == id);
            if (buyer == null)
            {
                return NotFound();
            }

            _context.Entry(buyer).State = EntityState.Modified;

            var nowUtc = DateTime.UtcNow;
            buyer._IsDeleted = true;
            buyer._DeletedUtc = nowUtc;

            await _context.SaveChangesAsync();

            return NoContent();
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
            buyer.Code = buyerVM.code ;
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
