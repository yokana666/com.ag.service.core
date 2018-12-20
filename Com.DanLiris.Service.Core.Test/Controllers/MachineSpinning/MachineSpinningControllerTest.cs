using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services.MachineSpinning;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Test.Utils;
using Com.DanLiris.Service.Core.WebApi.Controllers.v1.MachineSpinning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.MachineSpinning
{
    public class MachineSpinningControllerTest : BaseControllerTest<MachineSpinningController, MachineSpinningModel, MachineSpinningViewModel, IMachineSpinningService>
    {
        [Fact]
        public void UploadFile_WithoutException_ReturnOK()
        {
            //string header = "Merk Mesin, Nama, Jenis Mesin, Tahun Mesin, Kondisi Mesin, Kondisi Counter, Jumlah Delivery, Kapasitas/Hari, Satuan, Line, Unit";
            //var mocks = GetMocks();
            //mocks.Service.Setup()
            //mockFacade.Setup(f => f.UploadData(It.IsAny<List<COAModel>>())).Verifiable();
            //mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(',').ToList());

            //mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<COAViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(true, new List<object>()));
            //COAProfile profile = new COAProfile();

            //var mockMapper = new Mock<IMapper>();
            //mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));

            //mockMapper.Setup(x => x.Map<List<COAModel>>(It.IsAny<List<COAViewModel>>())).Returns(new List<COAModel>() { Model });
            //var mockIdentityService = new Mock<IIdentityService>();
            //var mockValidateService = new Mock<IValidateService>();

            //var controller = GetController((mockIdentityService, mockValidateService, mockFacade, mockMapper));
            //controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
            //controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            //var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + header)), 0, Encoding.UTF8.GetBytes(header + "\n" + header).LongLength, "Data", "test.csv");
            //controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

            //var response = controller.PostCSVFileAsync();
            //Assert.NotNull(response.Result);
        }

        [Fact]
        public void GetCSVTemplate_ReturnFile()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DownloadTemplate()).Returns(new MemoryStream());

            var response = GetController(mocks).DownloadTemplate();
            Assert.NotNull(response);
        }

        [Fact]
        public void GetCSVTemplate_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DownloadTemplate()).Throws(new Exception());

            var response = GetController(mocks).DownloadTemplate();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Get_Machine_Types()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetMachineTypes()).Returns(new List<string>());

            var response = GetController(mocks).GetMachineTypes();

            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }
    }
}
