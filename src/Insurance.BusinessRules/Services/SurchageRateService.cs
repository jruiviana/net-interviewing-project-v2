using System;
using System.Threading;
using System.Threading.Tasks;
using Insurance.Core.Exceptions;
using Insurance.Core.Models;
using Insurance.Data.Context;
using Insurance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.BusinessRules.Services
{
    public class SurchageRateService : ISurchageRateService
    {
        private readonly InsuranceContext _insuranceContext;

        public SurchageRateService(InsuranceContext insuranceContext)
        {
            _insuranceContext = insuranceContext;
        }

        public async Task AddOrUpdateInsuranceSurchargAsync(SurchargeRateDto surchargeRateDto, CancellationToken cancellationToken)
        {
            var surchargeRate = await _insuranceContext.SurchargeRates
                .FirstOrDefaultAsync(s => s.ProductTypeId == surchargeRateDto.ProductTypeId, cancellationToken);

            if (surchargeRate is null)
            {
                surchargeRate = new SurchargeRate
                {
                    Id = Guid.NewGuid(),
                    ProductTypeId = surchargeRateDto.ProductTypeId,
                    Rate = surchargeRateDto.Rate
                };
                await _insuranceContext.SurchargeRates.AddAsync(surchargeRate, cancellationToken);
            }
            else
            {
                surchargeRate.Rate = surchargeRateDto.Rate;
                _insuranceContext.SurchargeRates.Update(surchargeRate);
            }

            await _insuranceContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<decimal> GetInsuranceSurchargAsync(int productTypeId, CancellationToken cancellationToken)
        {
            var surchargeRate = await _insuranceContext.SurchargeRates
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ProductTypeId == productTypeId, cancellationToken);

            if (surchargeRate is null)
            {
                return 0;
            }

            return surchargeRate.Rate;
        }
    }
}