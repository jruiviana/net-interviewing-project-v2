using System.Text.Json.Serialization;

namespace Insurance.BusinessRules.Services.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("productTypeId")]
        public int ProductTypeId { get; set; }
        [JsonPropertyName("salesPrice")]
        public decimal SalesPrice { get; set; }
    }
}