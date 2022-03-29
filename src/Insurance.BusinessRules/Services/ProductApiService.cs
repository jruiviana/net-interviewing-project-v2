using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Insurance.BusinessRules.Services.Models;
using Insurance.Core.Exceptions;
using Insurance.Core.Models;
using Newtonsoft.Json;

namespace Insurance.BusinessRules.Services
{
    public class ProductApiService : IProductApiService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<InsuranceDto> GetProduct(int productId, CancellationToken cancellationToken)
        {
            var productTypes = await GetProductTypes(cancellationToken);

            var product = await GetApiProductAsync(productId, cancellationToken);

            var insurance = new InsuranceDto
            {
                ProductId = product.Id,
                SalesPrice = product.SalesPrice,
            };

            var productType = productTypes.FirstOrDefault(pt => pt.Id == product.ProductTypeId);

            insurance.ProductTypeName = productType.Name;
            insurance.ProductTypeHasInsurance = productType.CanBeInsured;
            insurance.ProductTypeId = productType.Id;
            
            return insurance;
        }

        private async Task<Product?> GetApiProductAsync(int productId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ProductApiClient");
            var response = await client.GetAsync($"/products/{productId:G}", cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ResourceNotFoundException();
            }
            var json= await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(json);
            return product;
        }

        private async  Task<List<ProductType>?> GetProductTypes(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ProductApiClient");
             var response = await client.GetAsync("/product_types", cancellationToken);
            var json= await response.Content.ReadAsStringAsync();
            var productTypes = JsonConvert.DeserializeObject<List<ProductType>>(json);
            return productTypes;
        }
    }
}