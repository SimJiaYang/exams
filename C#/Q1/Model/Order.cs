using Q1.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q1.Model
{
    /**
     * Order
     */
    public class Order : BaseEntity<long>
    {
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public int Status { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
    }
}
