using Q1.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using Q1.Services;

namespace Q1.Service
{
    public class CartService
    {
        private readonly List<CartItem> _cartItems = new List<CartItem>();
        private readonly ProductService _productService;
        private readonly PaymentService _paymentService;
        private readonly OrderService _orderService;

        public CartService(
            ProductService productService,
            PaymentService paymentService,
            OrderService orderService)
        {
            _productService = productService;
            _paymentService = paymentService;
            _orderService = orderService;
        }

        public void AddToCart(Product product, int quantity)
        {
            var existingItem = _cartItems.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.UpdatedOn = DateTimeOffset.Now;
                existingItem.UpdateBy = "System";
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Thumbnail = product.Images.FirstOrDefault() ?? string.Empty,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    UserId = "CurrentUser",
                    CreatedBy = "System",
                    CreatedOn = DateTimeOffset.Now,
                    UpdatedOn = DateTimeOffset.Now,
                    UpdateBy = "System"
                });
            }
        }

        public bool RemoveFromCart(long productId)
        {
            var itemToRemove = _cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                _cartItems.Remove(itemToRemove);
                return true;
            }
            return false;
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            return _cartItems.AsReadOnly();
        }

        public double GetCartTotal()
        {
            return _cartItems.Sum(item => item.TotalPrice);
        }

        public Order Checkout(string userId, string paymentMethod, string shippingAddress)
        {
            if (!_cartItems.Any())
                throw new InvalidOperationException("Cart is empty");

            foreach (var item in _cartItems)
            {
                var product = _productService.GetProduct(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product {item.ProductName} no longer available");

                if (product.Inventory < item.Quantity)
                    throw new InvalidOperationException($"Not enough inventory for {item.ProductName}");
            }

            var paymentResult = _paymentService.ProcessPayment(GetCartTotal(), paymentMethod);
            if (!paymentResult.Success)
                throw new InvalidOperationException($"Payment failed: {paymentResult.ErrorMessage}");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTimeOffset.Now,
                TotalAmount = GetCartTotal(),
                Status = 1,
                ShippingAddress = shippingAddress,
                PaymentMethod = paymentMethod,
                Items = _cartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList()
            };

            var createdOrder = _orderService.CreateOrder(order);

            foreach (var item in _cartItems)
            {
                var product = _productService.GetProduct(item.ProductId);
                product.Inventory -= item.Quantity;
            }

            _cartItems.Clear();

            return createdOrder;
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}