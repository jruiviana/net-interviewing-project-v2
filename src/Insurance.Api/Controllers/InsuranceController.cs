using System.Threading;
using System.Threading.Tasks;
using Insurance.BusinessRules.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "API Key")]
    public  class InsuranceController: ControllerBase
    {
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        /// <summary>
        /// Provides The product insurance
        /// </summary>
        /// <param name="productId">Product id.</param>
        /// <param name="cancellationToken">Cancellation of the request.</param>
        /// <returns></returns>
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetAsync(int productId, CancellationToken cancellationToken)
        {
            var insurance = await _insuranceService.GetProductInsuranceAsync(productId, cancellationToken);

            return Ok(insurance);
        }
        
        /// <summary>
        /// Provides The product insurance
        /// </summary>
        /// <param name="ids">Product id. in the format id1,id2,id3</param>
        /// <param name="cancellationToken">Cancellation of the request.</param>
        /// <returns></returns>
        [HttpGet("products")]
        public async Task<IActionResult> GetInsurancesAsync([FromQuery] string ids, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return BadRequest("ids is required");
            }

            var productIdList = ids.Split(",");
            var insurances = await _insuranceService.GetOrderInsuranceAsync(productIdList, cancellationToken);

            return Ok(insurances);
        }
    }
}