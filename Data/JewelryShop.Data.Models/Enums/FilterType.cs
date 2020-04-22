namespace JewelryShop.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum FilterType
    {
        [Display(Name = "Всички")]
        All = 0,
        [Display(Name = "Архивиран")]
        Archived = 1,
        [Display(Name = "Изчерпани")]
        OutOfStock = 2,
        [Display(Name = "Налични")]
        Stock = 3,
    }
}
