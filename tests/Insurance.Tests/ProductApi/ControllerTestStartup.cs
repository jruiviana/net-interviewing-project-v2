using System.Collections.Generic;
using System.Linq;
using System.Net;
using FizzWare.NBuilder;
using Insurance.BusinessRules.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Insurance.Tests.ProductApi
{
    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            var productId = int.Parse((string) context.Request.RouteValues["id"]);
                            var products = GenerateTestProducts();
                            var product = products.FirstOrDefault(p => p.Id == productId);
                            if (product is null)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                return context.Response.WriteAsync("Not Found");
                            }
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context =>
                        {
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(GenerateTestProductTypes()));
                        }
                    );
                }
            );
        }

        private IEnumerable<Product> GenerateTestProducts()
        {
            var products = Builder<Product>.CreateListOfSize(9)
                .TheFirst(1)
                .With(x => x.Id = 1)
                .With(x => x.ProductTypeId = 3)
                .With(x => x.SalesPrice = 300)
                .TheNext(1)
                .With(x => x.Id = 2)
                .With(x => x.ProductTypeId = 3)
                .With(x => x.SalesPrice = 750)
                .TheNext(1)
                .With(x => x.Id = 3)
                .With(x => x.ProductTypeId = 3)
                .With(x => x.SalesPrice = 3000)
                .TheNext(1)
                .With(x => x.Id = 4)
                .With(x => x.ProductTypeId = 1)
                .With(x => x.SalesPrice = 600)
                .TheNext(1)
                .With(x => x.Id = 5)
                .With(x => x.ProductTypeId = 2)
                .With(x => x.SalesPrice = 1000)
                .TheNext(1)
                .With(x => x.Id = 6)
                .With(x => x.ProductTypeId = 1)
                .With(x => x.SalesPrice = 300)
                .TheNext(1)
                .With(x => x.Id = 7)
                .With(x => x.ProductTypeId = 3)
                .With(x => x.SalesPrice = 500)
                .TheNext(1)
                .With(x => x.Id = 8)
                .With(x => x.ProductTypeId = 4)
                .With(x => x.SalesPrice = 500)
                .TheNext(1)
                .With(x => x.Id = 9)
                .With(x => x.ProductTypeId = 5)
                .With(x => x.SalesPrice = 1000)
                .Build();
            return products;
        }
        private IEnumerable<ProductType> GenerateTestProductTypes()
        {
            var productTypes = Builder<ProductType>.CreateListOfSize(5)
                .TheFirst(1)
                .With(x => x.Id = 1)
                .With(x => x.Name = "Smartphones")
                .With(x => x.CanBeInsured = true)
                .TheNext(1)
                .With(x => x.Id = 2)
                .With(x => x.Name = "Laptops")
                .With(x => x.CanBeInsured = true)
                .TheNext(1)
                .With(x => x.Id = 3)
                .With(x => x.Name = "game")
                .With(x => x.CanBeInsured = true)
                .TheNext(1)
                .With(x => x.Id = 4)
                .With(x => x.Name = "Digital cameras")
                .With(x => x.CanBeInsured = true)
                .TheNext(1)
                .With(x => x.Id = 5)
                .With(x => x.Name = "desktops")
                .With(x => x.CanBeInsured = true)
                .Build();
            return productTypes;
        }
    }
}