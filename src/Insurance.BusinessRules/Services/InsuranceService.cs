using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Insurance.Core.Exceptions;
using Insurance.Core.Models;
using Insurance.Data.Context;
using Insurance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.BusinessRules.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IProductApiService _productApiService;
        private readonly ISurchageRateService _ISurchageRateService;
        
        public InsuranceService(IProductApiService productApiService, ISurchageRateService iSurchageRateService)
        {
            _productApiService = productApiService;
            _ISurchageRateService = iSurchageRateService;
        }


        public async Task<InsuranceDto> GetProductInsuranceAsync(int productId, CancellationToken cancellationToken)
        {
            var productInsurance = await _productApiService.GetProduct(productId, cancellationToken);

            return await CalculateInsuranceAsync(productInsurance, cancellationToken);
        }

        public async Task<OrderInsuranceDto> GetOrderInsuranceAsync(IEnumerable<string> productIds,
            CancellationToken cancellationToken)
        {
            var orderInsuranceDto = new OrderInsuranceDto()
            {
                ProductInsurances = new List<InsuranceDto>()
            };

            foreach (var id in productIds)
            {
                if (!int.TryParse(id, out var productId))
                {
                    throw new BadRequestException($"product id {id} is not in the correct format");
                }

                var productInsurance = await GetProductInsuranceAsync(productId, cancellationToken);
                orderInsuranceDto.ProductInsurances.Add(productInsurance);
                orderInsuranceDto.TotalInsuranceValue += productInsurance.InsuranceValue;
            }

            AddExtraInsuranceforDigitalCameras(orderInsuranceDto);

            return orderInsuranceDto;
        }

        private static void AddExtraInsuranceforDigitalCameras(OrderInsuranceDto? orderInsuranceDto)
        {
            if (orderInsuranceDto.ProductInsurances.Any(x => x.ProductTypeName.Equals("Digital cameras")))
            {
                orderInsuranceDto.TotalInsuranceValue += 500;
            }
        }

        private async Task <InsuranceDto> CalculateInsuranceAsync(InsuranceDto? productInsurance, CancellationToken cancellationToken)
        {
            if (productInsurance.SalesPrice < 500)
                productInsurance.InsuranceValue = 0;
            else
            {
                if (productInsurance.SalesPrice >= 500
                    && productInsurance.SalesPrice < 2000)
                {
                    if (productInsurance.ProductTypeHasInsurance)
                    {
                        productInsurance.InsuranceValue += 1000;
                    }
                }

                if (productInsurance.SalesPrice >= 2000)
                {
                    if (productInsurance.ProductTypeHasInsurance)
                    {
                        productInsurance.InsuranceValue += 2000;
                    }
                }
            }

            if (productInsurance.ProductTypeName.Equals("Laptops")
                || productInsurance.ProductTypeName.Equals("Smartphones")
                && productInsurance.ProductTypeHasInsurance)
            {
                productInsurance.InsuranceValue += 500;
            }

            await AddSurchargeRate(productInsurance, cancellationToken);

            return productInsurance;
        }

        private async Task AddSurchargeRate(InsuranceDto productInsurance, CancellationToken cancellationToken)
        {
            var insuranceSurchargeRate =
                await _ISurchageRateService.GetInsuranceSurchargAsync(productInsurance.ProductTypeId, cancellationToken);

            productInsurance.InsuranceValue += (productInsurance.InsuranceValue * insuranceSurchargeRate);
        }
    }
}