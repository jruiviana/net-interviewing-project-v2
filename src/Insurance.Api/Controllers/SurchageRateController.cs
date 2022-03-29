using System.Threading;
using System.Threading.Tasks;
using Insurance.BusinessRules.Services;
using Insurance.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "API Key")]
    public  class SurchageRateController: ControllerBase
    {
        private readonly ISurchageRateService _surchageRateService;

        public SurchageRateController(ISurchageRateService surchageRateService)
        {
            _surchageRateService = surchageRateService;
        }

        /// <summary>
        /// Ser a surchage rate for a product type
        /// </summary>
        /// <param name="surchargeRate">Product type id and rate.</param>
        /// <param name="cancellationToken">Cancellation of the request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SurchargeRateDto surchargeRate, CancellationToken cancellationToken)
        {
            if (surchargeRate is null)
            {
                return BadRequest("surchargeRate is required");
            }
            await _surchageRateService.AddOrUpdateInsuranceSurchargAsync(surchargeRate, cancellationToken);

            return Ok();
        }
    }
}