using System;
using System.Collections.Generic;
using Q1.Model;
using Q1.Services;
using Q1.Service;
using System.Linq;

namespace Q1
{
    internal class Program
    {
        private static ProductService _productService;
        private static CartService _cartService;
        private static PaymentService _paymentService;
        private static OrderService _orderService;

        /**
         * Program process flow
         * 1. Service initialization
         * 2. Create prodct seed
         * 3. Show selection interface
         *    - View Products 
         *    - View Carts
         *    - Add Product to Cart
         *    - Remove Product from Cart
         *    - Checkout
         *    - Exit
         */
        static void Main(string[] args)
        {
            InitializeServices();
            SeedProducts();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Shopping Cart System ===");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. View Cart");
                Console.WriteLine("3. Add Product to Cart");
                Console.WriteLine("4. Remove Product from Cart");
                Console.WriteLine("5. Checkout");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ViewProducts();
                        break;
                    case "2":
                        ViewCart();
                        break;
                    case "3":
                        AddToCart();
                        break;
                    case "4":
                        RemoveFromCart();
                        break;
                    case "5":
                        Checkout();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void InitializeServices()
        {
            _productService = new ProductService();
            _paymentService = new PaymentService();
            _orderService = new OrderService();
            _cartService = new CartService(_productService, _paymentService, _orderService);
        }

        static void SeedProducts()
        {
            _productService.AddProduct(new Product
            {
                Id = 1,
                Name = "Wireless Headphones",
                Description = "Noise cancelling Bluetooth headphones",
                Images = new[] { "headphones.jpg" },
                Price = 199.99,
                Inventory = 50,
                SKU = "WH-1000XM4",
                Status = 1,
                MerchantId = 1,
                CategoryId = 1
            });

            _productService.AddProduct(new Product
            {
                Id = 2,
                Name = "Smart Watch",
                Description = "Fitness tracker with heart rate monitor",
                Images = new[] { "watch.jpg" },
                Price = 129.99,
                Inventory = 30,
                SKU = "SW-FIT-2023",
                Status = 1,
                MerchantId = 1,
                CategoryId = 2
            });

            _productService.AddProduct(new Product
            {
                Id = 3,
                Name = "USB-C Cable",
                Description = "High speed charging cable 1m",
                Images = new[] { "cable.jpg" },
                Price = 15.99,
                Inventory = 100,
                SKU = "USB-C-1M",
                Status = 1,
                MerchantId = 2,
                CategoryId = 3
            });
        }

        static void ViewProducts()
        {
            Console.WriteLine("\n=== Available Products ===");
            foreach (var product in _productService.GetAllProducts())
            {
                Console.WriteLine(product.ToString());
            }
        }

        static void ViewCart()
        {
            var items = _cartService.GetCartItems();
            Console.WriteLine("\n=== Your Shopping Cart ===");

            if (!items.Any())
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine($"{item.ProductName} - {item.Quantity} x RM{item.UnitPrice} = RM{item.TotalPrice}");
            }

            Console.WriteLine($"\nTotal: RM{_cartService.GetCartTotal()}");
        }

        static void AddToCart()
        {
            ViewProducts();
            Console.Write("\nEnter product ID to add to cart: ");
            if (!long.TryParse(Console.ReadLine(), out long productId))
            {
                Console.WriteLine("Invalid product ID.");
                return;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }

            var product = _productService.GetProduct(productId);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            if (product.Inventory < quantity)
            {
                Console.WriteLine($"Only {product.Inventory} available in stock.");
                return;
            }

            _cartService.AddToCart(product, quantity);
            Console.WriteLine($"Added {quantity} {product.Name}(s) to cart.");
        }

        static void RemoveFromCart()
        {
            var items = _cartService.GetCartItems();
            if (!items.Any())
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            ViewCart();
            Console.Write("\nEnter product ID to remove from cart: ");
            if (!long.TryParse(Console.ReadLine(), out long productId))
            {
                Console.WriteLine("Invalid product ID.");
                return;
            }

            if (_cartService.RemoveFromCart(productId))
            {
                Console.WriteLine("Product removed from cart.");
            }
            else
            {
                Console.WriteLine("Product not found in cart.");
            }
        }

        static void Checkout()
        {
            try
            {
                Console.WriteLine("\n=== Checkout ===");
                ViewCart();

                if (!_cartService.GetCartItems().Any())
                {
                    Console.WriteLine("Cannot checkout with an empty cart.");
                    return;
                }

                Console.Write("\nEnter shipping address: ");
                var address = Console.ReadLine();

                Console.Write("Enter payment method (CreditCard/PayPal): ");
                var paymentMethod = Console.ReadLine();

                var userId = "user123";

                var order = _cartService.Checkout(userId, paymentMethod, address);

                Console.WriteLine($"\nOrder #{order.OrderNumber} created successfully!");
                Console.WriteLine($"Total: RM{order.TotalAmount}");
                Console.WriteLine($"Shipping to: {order.ShippingAddress}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Checkout failed: {ex.Message}");
            }
        }
    }
}