using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Insurance.Api.Controllers;
using Insurance.BusinessRules.Services;
using Insurance.Core.Models;
using Insurance.Data.Context;
using Insurance.Specs.ProductApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Insurance.Specs.Steps
{
    [Binding]
    public sealed class InsuranceStepDefinitions : BaseStep,IClassFixture<ControllerTestFixture>
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ControllerTestFixture _fixture;
        private readonly InsuranceController _insuranceController;
        private IActionResult _result;
        private int _productId;

        public InsuranceStepDefinitions(ScenarioContext scenarioContext, ControllerTestFixture fixture)
        {
            _scenarioContext = scenarioContext;
            _fixture = fixture;
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5002"),
            };
           
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var dbContext = CreateDbContext<InsuranceContext>();
            var productApiService = new ProductApiService(mockFactory.Object);
            var surchageRateService = new SurchageRateService(dbContext);
            var insuranceService = new InsuranceService(productApiService, surchageRateService);
            _insuranceController = new InsuranceController(insuranceService);
        }

        [Given("product id (.*)")]
        public void GivenProductId(int productID)
        {
            _productId = productID;
        }

        [When("the Insurance is calculated")]
        public async Task WhenTheInsuranceIsCalculated()
        {
            _result = await _insuranceController.GetAsync(_productId, default);
        }

        [Then("no insurance required")]
        public void ThenNoInsuranceRequired()
        {
            var okResult = _result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;
            
            Assert.Equal(expected: _productId, actual: insurance!.ProductId);
            Assert.Equal(expected: 0, actual: insurance.InsuranceValue);
        }
        
        [Then("the insurance should be (.*)")]
        public void ThenTheInsuranceShouldBe(decimal insuranceValue)
        {
            var okResult = _result as OkObjectResult;
            var insurance = okResult!.Value as InsuranceDto;
            
            Assert.Equal(expected: _productId, actual: insurance!.ProductId);
            Assert.Equal(expected: insuranceValue, actual: insurance.InsuranceValue);
        }
    }
}