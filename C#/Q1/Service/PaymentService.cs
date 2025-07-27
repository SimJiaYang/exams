using Q1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q1.Service
{
    public class PaymentService
    {
        public PaymentResult ProcessPayment(double amount, string paymentMethod)
        {
            Console.WriteLine($"Processing {paymentMethod} payment for ${amount}...");

            return new PaymentResult
            {
                Success = true,
                TransactionId = Guid.NewGuid().ToString(),
                ErrorMessage = null
            };
        }
    }
}
