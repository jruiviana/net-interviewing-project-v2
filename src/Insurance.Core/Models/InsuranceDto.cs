using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Insurance.Core.Models
{
    public class InsuranceDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("insuranceValue")]
        public decimal InsuranceValue { get; set; }
        [JsonIgnore] public string ProductTypeName { get; set; }
        [JsonIgnore] public int ProductTypeId { get; set; }
        [JsonIgnore] public bool ProductTypeHasInsurance { get; set; }
        [JsonIgnore] public decimal SalesPrice { get; set; }
    }
}