using System.Threading;
using System.Threading.Tasks;
using Insurance.Core.Models;

namespace Insurance.BusinessRules.Services
{
    public interface ISurchageRateService
    {
        Task AddOrUpdateInsuranceSurchargAsync(SurchargeRateDto surchargeRateDto, CancellationToken cancellationToken);
        Task<decimal> GetInsuranceSurchargAsync(int productTypeId, CancellationToken cancellationToken);
    }
}