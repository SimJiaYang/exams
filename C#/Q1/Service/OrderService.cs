using Q1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q1.Service
{
    public class OrderService
    {
        private long _nextOrderId = 1;

        public Order CreateOrder(Order order)
        {
            order.Id = _nextOrderId++;
            order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{order.Id.ToString().PadLeft(5, '0')}";
            order.CreatedBy = "System";
            order.CreatedOn = DateTimeOffset.Now;
            order.UpdatedOn = DateTimeOffset.Now;
            order.UpdateBy = "System";

            Console.WriteLine($"Created order {order.OrderNumber} for {order.UserId}");

            return order;
        }
    }
}
