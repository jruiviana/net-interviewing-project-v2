using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Controllers;
using Insurance.BusinessRules.Services;
using Insurance.Core.Models;
using Insurance.Data.Context;
using Insurance.Data.Entities;
using Insurance.Tests.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Insurance.Tests
{
    public class SurchargeTests: BaseTest
    {
        private readonly SurchageRateController _surchageRateController;
        private readonly InsuranceContext _dbContext;
        
        public SurchargeTests()
        {
            _dbContext = CreateDbContext<InsuranceContext>();
            var surchageRateService = new SurchageRateService(_dbContext);
            _surchageRateController = new SurchageRateController(surchageRateService);
        }

        [Fact] //Task 5 unit test
        public async Task SurchargeRate_GivenNewSurchargeRate_ShouldAddInTheDatabase()
        {
            var surcharge = new SurchargeRateDto()
            {
                ProductTypeId = 3,
                Rate = 0.10M
            };
            
            await _surchageRateController.PostAsync(surcharge, default);

            var surchargeRate = _dbContext.SurchargeRates.FirstOrDefault(x => x.ProductTypeId == 3);

            Assert.Equal(expected: 0.10M , actual:surchargeRate.Rate);
        }
        
        [Fact] //Task 5 unit test
        public async Task SurchargeRate_GivenExistingSurchargeRate_ShouldUpdateInTheDatabase()
        {
            _dbContext.SurchargeRates.Add(new SurchargeRate()
            {
                ProductTypeId = 3,
                Rate = 0.10M
            });

            _dbContext.SaveChanges();
                
            var surcharge = new SurchargeRateDto()
            {
                ProductTypeId = 3,
                Rate = 0.20M
            };
            
            await _surchageRateController.PostAsync(surcharge, default);

            var surchargeRate = _dbContext.SurchargeRates.FirstOrDefault(x => x.ProductTypeId == 3);

            Assert.Equal(expected: 0.20M , actual:surchargeRate.Rate);
        }
    }
}