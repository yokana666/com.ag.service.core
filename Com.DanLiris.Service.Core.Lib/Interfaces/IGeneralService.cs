using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Interfaces
{
    public interface IGeneralService<TModel>
    {
        Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> Read(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null);
    }
}
