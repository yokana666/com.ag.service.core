using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.IntegrationService
{
    public interface IProductIntegrationService
    {
        Task<int> IntegrateData();
    }
}