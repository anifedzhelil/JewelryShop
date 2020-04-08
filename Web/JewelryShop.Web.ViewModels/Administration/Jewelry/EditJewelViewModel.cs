namespace JewelryShop.Web.ViewModels.Administration.Jewelry
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.ValidationAttributes;

    public class EditJewelViewModel : IMapFrom<Jewel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля въведете име")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Моля въведете цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моля въведете описание")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [SalePriceLessThan("Price", "SaleDate")]
        public decimal? SalePrice { get; set; }

        [SaleDateLessThan("SalePrice")]
        [DataType(DataType.DateTime)]
        public DateTime? SaleDate { get; set; }

        [Required(ErrorMessage = "Моля въведете брой")]
        public int Count { get; set; }

        [Required(ErrorMessage = "Моля  изберете категория")]
        public int Category { get; set; }

        public bool IsArchived { get; set; }

        public IEnumerable<string> Images { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Jewel, EditJewelViewModel>()
            .ForMember(
                d => d.Images,
                opt => opt.MapFrom(x => x.Images.Select(t => t.ImageUrl)));
        }
    }
}
