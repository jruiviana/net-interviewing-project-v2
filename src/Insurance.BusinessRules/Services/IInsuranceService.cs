using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insurance.Core.Models;

namespace Insurance.BusinessRules.Services
{
    public interface IInsuranceService
    {
        Task<InsuranceDto> GetProductInsuranceAsync(int productId, CancellationToken cancellationToken);
        Task<OrderInsuranceDto> GetOrderInsuranceAsync(IEnumerable<string> productIds, CancellationToken cancellationToken);
    }
}