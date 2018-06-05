using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Test.DataUtils
{
    public class BudgetServiceDataUtil
    {
        public CoreDbContext DbContext { get; set; }

        public BudgetService BudgetService { get; set; }

        public BudgetServiceDataUtil(CoreDbContext dbContext, BudgetService budgetService)
        {
            this.DbContext = dbContext;
            this.BudgetService = budgetService;
        }
        
        public Task<Budget> GetTestBuget()
        {
            Budget testBudget = BudgetService.DbSet.FirstOrDefault(budget => budget.Code.Equals("Test"));

            if (testBudget != null)
                return Task.FromResult(testBudget);
            else
            {
                testBudget = new Budget()
                {
                    Code = "Test",
                    Name = "Test Budget"
                };

                int id = BudgetService.Create(testBudget);
                return BudgetService.GetAsync(id);
            }
        }
    }
}