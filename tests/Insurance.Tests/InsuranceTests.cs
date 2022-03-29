using System;
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
    public class InsuranceTests: BaseTest, IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;
        private readonly InsuranceController _insuranceController;
        

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5002"),
            };
           
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var dbContext = CreateDbContext<InsuranceContext>();
            AddDbTestData(dbContext);
            var productApiService = new ProductApiService(mockFactory.Object);
            var surchageRateService = new SurchageRateService(dbContext);
            var insuranceService = new InsuranceService(productApiService, surchageRateService);
            _insuranceController = new InsuranceController(insuranceService);
        }

        [Fact]
        public async Task CalculateInsurance_GivenNotExistProduct_ShouldReturnNotFoundException()
        {
            await Assert.ThrowsAsync<Insurance.Core.Exceptions.ResourceNotFoundException>(() => _insuranceController.GetAsync(10, default));
        }
        
        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceBetweenLessThan500Euros_ShouldHaveNoInsurance()
        {
            const decimal expectedInsuranceValue = 0;

            var result = await _insuranceController.GetAsync(1, default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;

            Assert.Equal(expected: 1, actual: insurance!.ProductId);
            Assert.Equal(expected: expectedInsuranceValue, actual: insurance.InsuranceValue);
        }
        
        [Theory]
        [InlineData(2, 1000)] //Product with price 750
        [InlineData(7, 1000)] //Product with price 500
        public async Task CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost(int productId, decimal expectedInsuranceValue)
        {
            var result = await _insuranceController.GetAsync(productId, default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;
        
            Assert.Equal(expected: productId, actual: insurance!.ProductId);
            Assert.Equal(expected: expectedInsuranceValue, actual: insurance.InsuranceValue);
        }
        
        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceGreaterThan2000Euros_ShouldSet2000EurosToInsuranceCost()
        {
            const decimal expectedInsuranceValue = 2000;
        
            var result = await _insuranceController.GetAsync(3, default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;
        
            Assert.Equal(expected: 3, actual: insurance!.ProductId);
            Assert.Equal(expected: expectedInsuranceValue, actual: insurance.InsuranceValue);
        }
        
        [Theory]
        [InlineData(4, 1500)] //Product SmartPhone
        [InlineData(5, 1500)] //Product Laptop
        [InlineData(6, 500)] //Product smartphone with SalesPric below 500
        public async Task CalculateInsurance_GivenProductSmartPhoneOrLaptop_ShouldAdd500EurosToInsuranceCost(int productId, decimal expectedInsuranceValue)
        {
            var result = await _insuranceController.GetAsync(productId, default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;
        
            Assert.Equal(expected: productId, actual: insurance!.ProductId);
            Assert.Equal(expected: expectedInsuranceValue, actual: insurance.InsuranceValue);
        }
        
        [Fact] //Task 3 unit test
        public async Task CalculateInsurance_GivenMoreThanOneProductAndSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            var result = await _insuranceController.GetInsurancesAsync("2,7", default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as OrderInsuranceDto;
        
            Assert.Equal(expected: 2, actual: insurance.ProductInsurances.Count);
            Assert.Equal(expected: 1000, actual: insurance.ProductInsurances[0].InsuranceValue);
            Assert.Equal(expected: 1000, actual: insurance.ProductInsurances[1].InsuranceValue);
            Assert.Equal(expected: 2000, actual: insurance.TotalInsuranceValue);
        }
        
        [Fact] //Task 4 unit test
        public async Task CalculateInsurance_GivenMoreThanOneProductAndOrderHaveDigitalCameras_ShouldAdd500eurosToInsuranceCost()
        {
            var result = await _insuranceController.GetInsurancesAsync("2,7,8", default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as OrderInsuranceDto;
        
            Assert.Equal(expected: 3, actual: insurance.ProductInsurances.Count);
            Assert.Equal(expected: 1000, actual: insurance.ProductInsurances[0].InsuranceValue);
            Assert.Equal(expected: 1000, actual: insurance.ProductInsurances[1].InsuranceValue);
            Assert.Equal(expected: 1000, actual: insurance.ProductInsurances[2].InsuranceValue);
            Assert.Equal(expected: 3500, actual: insurance.TotalInsuranceValue);
        }
        
        [Fact] //Task 5 unit test
        public async Task CalculateInsurance_GivenOneProductWith10PercentSurchageRate_ShouldApplyRateToInsuranceCost()
        {
            var result = await _insuranceController.GetAsync(9, default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;
        
            Assert.Equal(expected: 1100, actual: insurance.InsuranceValue);
          
        }
        
        [Fact] //Task 5 unit test
        public async Task CalculateInsurance_GivenOneProductWithNoSurchageRate_ShouldApplyRateNoToInsuranceCost()
        {
            var result = await _insuranceController.GetAsync(5, default);
            var okResult = result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;
        
            Assert.Equal(expected: 1500, actual: insurance.InsuranceValue);
          
        }

        private void AddDbTestData(InsuranceContext context)
        {
            context.SurchargeRates.Add(new SurchargeRate()
            {
                Id = Guid.NewGuid(),
                ProductTypeId = 5,
                Rate = 0.10M,
            });

            context.SaveChanges();
        }
    }
}