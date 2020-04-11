﻿namespace JewelryShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using JewelryShop.Data.Common.Models;

    public class Jewel : BaseDeletableModel<int>
    {
        public Jewel()
        {
            this.Images = new HashSet<JewelImages>();
            this.Ratings = new HashSet<Rating>();
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public CategoryType Category { get; set; }

        public decimal? SalePrice { get; set; }

        public DateTime? SaleDate { get; set; }

        public int Count { get; set; }

        public bool IsArchived { get; set; }

        public virtual ICollection<JewelImages> Images { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
