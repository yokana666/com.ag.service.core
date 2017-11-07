using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.WebApi
{
    public class ResultFormatter<TModel, TViewModel>
    {
        public delegate TViewModel MapCallBack(TModel model);
        public ResultFormatter(List<TModel> data, MapCallBack map)
        {
            List<TViewModel> viewModel = new List<TViewModel>();

            foreach (TModel d in data)
            {
                viewModel.Add(map(d)); ;
            }
        }
    }
}
