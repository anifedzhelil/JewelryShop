namespace JewelryShop.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class IndexJewelryViewModel : IMapFrom<Jewel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public DateTime SaleDate { get; set; }

        public string ImageUrl { get; set; }

        public double Ratings { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Jewel, IndexJewelryViewModel>()
            .ForMember(
                d => d.ImageUrl,
                opt => opt.MapFrom(x => x.Images.Select(t => t.ImageUrl).FirstOrDefault()))
              .ForMember(
               d => d.Ratings,
               opt => opt.MapFrom(x => x.Ratings.Average(t => (double)t.Type)));
        }
    }
}
