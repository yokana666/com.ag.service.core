using Com.DanLiris.Service.Core.Lib.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class BudgetService : StandardEntityService<CoreDbContext, Budget>
    {
        public BudgetService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
