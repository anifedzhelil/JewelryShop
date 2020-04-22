namespace JewelryShop.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum SortType
    {
        [Display(Name = "Най-нови")]
        CreatedDate = 0,
        [Display(Name = "Най-продаван")]
        BestSelling = 1,
        [Display(Name = "Рейтинг")]
        Rating = 2,
        [Display(Name = "Най-висока цена")]
        HighPrice = 3,
        [Display(Name = "Най-ниска цема")]
        LowPrice = 4,
    }
}
