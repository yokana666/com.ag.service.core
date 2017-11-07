using Com.DanLiris.Service.Core.Lib.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class BuyerService : StandardEntityService<CoreDbContext, Buyer>
    {
        public BuyerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
