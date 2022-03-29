using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Insurance.Core.Models
{
    public class OrderInsuranceDto
    {
        [JsonPropertyName("productInsurances")]
        public IList<InsuranceDto> ProductInsurances { get; set; }
        [JsonPropertyName("totalInsuranceValue")]
        public decimal TotalInsuranceValue { get; set; }
    }
}