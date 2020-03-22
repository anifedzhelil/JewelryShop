namespace JewelryShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using JewelryShop.Data.Common.Models;

    public class JewelImages : BaseModel<int>
    {
        public string ImageUrl { get; set; }

        public int JewelId { get; set; }

        public virtual Jewel Jewel { get; set; }
    }
}
