using Q1.Base;
using System;

namespace Q1.Model
{
    public class Product : BaseEntity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; } = Array.Empty<string>();
        public double Price { get; set; }
        public int Inventory { get; set; }
        public string SKU { get; set; }
        public int Status { get; set; }

        public long MerchantId { get; set; }
        public long CategoryId { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name} - RM{Price} ({Inventory} in stock)";
        }
    }
}