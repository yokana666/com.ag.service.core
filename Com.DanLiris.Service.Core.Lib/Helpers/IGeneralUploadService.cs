using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Helpers
{
    public interface IGeneralUploadService<TModel>
    {
        Tuple<bool, List<object>> UploadValidate(List<TModel> Data);

        List<string> CsvHeader { get; }
    }
}
