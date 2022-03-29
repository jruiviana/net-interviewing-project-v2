using System.Threading;
using System.Threading.Tasks;
using Insurance.Core.Models;

namespace Insurance.BusinessRules.Services
{
    public interface IProductApiService
    {
        Task<InsuranceDto> GetProduct(int productId, CancellationToken cancellationToken);
    }
}