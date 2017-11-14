using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Com.DanLiris.Service.Core.WebApi.Helpers;
using CsvHelper;
using CsvHelper.Configuration;

namespace Com.DanLiris.Service.Core.WebApi.v1.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/master/upload-buyers")]
    public class UploadBuyersController : Controller
    {
        private readonly string ApiVersion = "1.0";
        private readonly List<string> Header = new List<string>()
        {
            "Kode Buyer", "Nama", "Alamat", "Kota", "Negara", "NPWP", "Jenis Buyer", "Kontak", "Tempo"
        };

        private class Buyer
        {
            public string KodeBuyer { get; set; }

            public string Nama { get; set; }

            public string Alamat { get; set; }

            public string Kota { get; set; }

            public string Negara { get; set; }

            public string NPWP { get; set; }

            public string JenisBuyer { get; set; }

            public string Kontak { get; set; }

            public int? Tempo { get; set; }
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
                    CsvReader Csv = new CsvReader(Reader);
                    Csv.Configuration.HasHeaderRecord = false;
                    Csv.Configuration.IgnoreQuotes = false;
                    Csv.Configuration.Delimiter = ",";
                    
                    var Data = Csv.GetRecords<Buyer>().ToList();
                    
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
            catch(Exception e)
            {
                Dictionary<string, object> Result =
                   new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.INTERNAL_ERROR_MESSAGE)
                   .Fail(e);

                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}