using Q1.Base;

namespace Q1.Model
{
    /**
     * Cart item
     */
    public class CartItem : BaseEntity<long>
    {
        public string ProductName { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

        public long ProductId { get; set; }
        public string UserId { get; set; }

        /**
        * Calculate total price per item
        */
        public double TotalPrice => UnitPrice * Quantity;
    }
}

