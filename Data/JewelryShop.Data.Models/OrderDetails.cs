using JewelryShop.Data.Common.Models;

namespace JewelryShop.Data.Models
{
    public class OrderDetails : BaseDeletableModel<int>
    {
        public int OrderId { get; set; }

        public int JewelId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public virtual Order Order { get; set; }
    }
}
