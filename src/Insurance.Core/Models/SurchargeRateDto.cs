using System.Text.Json.Serialization;

namespace Insurance.Core.Models
{
    public class SurchargeRateDto
    {
        [JsonPropertyName("productTypeId")]
        public int ProductTypeId { get; set; }
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}