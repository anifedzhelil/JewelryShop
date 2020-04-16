namespace JewelryShop.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum CategoryType
    {
        [Display(Name = "Гривни")]
        Bracelet = 1,
        [Display(Name = "Колиета")]
        Necklace = 2,
        [Display(Name = "Обеци")]
        Earrings = 3,
        [Display(Name = "Брошки")]
        Brooch = 4,
        [Display(Name = "Комплекти")]
        Set = 5,
    }
}
