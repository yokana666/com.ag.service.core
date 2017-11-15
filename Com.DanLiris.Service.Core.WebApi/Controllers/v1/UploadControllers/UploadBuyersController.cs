using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Com.DanLiris.Service.Core.WebApi.Helpers;
using CsvHelper;
using CsvHelper.Configuration;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.NetCore.Lib.Service;

namespace Com.DanLiris.Service.Core.WebApi.Controllers.v1.UploadControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/master/upload-buyers")]
    public class UploadBuyersController : Controller
    {
        private readonly CoreDbContext _context;
        private readonly BuyerService _service;
        private readonly string ApiVersion = "1.0";

        public UploadBuyersController(CoreDbContext context, BuyerService service)
        {
            _context = context;
            _service = service;
        }
        private readonly List<string> Header = new List<string>()
        {
            "Kode Buyer", "Nama", "Alamat", "Kota", "Negara", "NPWP", "Jenis Buyer", "Kontak", "Tempo"
        };

        public sealed class BuyerMap : ClassMap<Buyer>
        {
            public BuyerMap()
            {
                Map(b => b.Code).Name("Kode Buyer");
                Map(b => b.Name).Name("Nama");
                Map(b => b.Address).Name("Alamat");
                Map(b => b.City).Name("Kota");
                Map(b => b.Country).Name("Negara");
                Map(b => b.NPWP).Name("NPWP");
                Map(b => b.Type).Name("Jenis Buyer");
                Map(b => b.Contact).Name("Kontak");
                Map(b => b.Tempo).Name("Tempo");
            }
        }

        [HttpPost]
        public IActionResult PostUploadBuyers()
        {
            try
            {
                var UploadedFile = Request.Form.Files[0];
                StreamReader Reader = new StreamReader(UploadedFile.OpenReadStream());
                List<string> FileHeader = new List<string>(Reader.ReadLine().Split(","));
                var ValidHeader = Header.SequenceEqual(FileHeader);
                
                if (ValidHeader)
                {
                    Reader.DiscardBufferedData();
                    Reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    Reader.BaseStream.Position = 0;

                    CsvReader Csv = new CsvReader(Reader);
                    Csv.Configuration.IgnoreQuotes = false;
                    Csv.Configuration.Delimiter = ",";
                    Csv.Configuration.RegisterClassMap<BuyerMap>();
                    
                    List<Buyer> Data = Csv.GetRecords<Buyer>().ToList();

                    using (var Transaction = _context.Database.BeginTransaction())
                    {
                        foreach (Buyer buyer in Data)
                        {
                            _service.Create(buyer);
                        }

                        Transaction.Commit();
                    }
                    
                    return NoContent();
                }
                else
                {
                    Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.CSV_ERROR_MESSAGE)
                       .Fail();

                    return NotFound(Result);
                }
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(e);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                   new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                   .Fail(e);

                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}