using System;

namespace Insurance.Data.Entities
{
    public class SurchargeRate
    {
        public Guid Id { set; get; }
        public int ProductTypeId { set; get; }
        public decimal Rate { set; get; }
    }
}