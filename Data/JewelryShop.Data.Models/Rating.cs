namespace JewelryShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using JewelryShop.Data.Common.Models;
    using JewelryShop.Data.Models.Enums;

    public class Rating : BaseModel<int>
    {
        public int JewelId { get; set; }

        public virtual Jewel Jewel { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string? Review { get; set; }

        public RatingType Type { get; set; }
    }
}
