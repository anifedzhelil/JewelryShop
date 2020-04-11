namespace JewelryShop.Web.ViewModels.ProductDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.Ratings;

    public class IndexViewModel : IMapFrom<Jewel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public DateTime SaleDate { get; set; }

        public int Count { get; set; }

        public IEnumerable<string> Images { get; set; }

        public double Ratings { get; set; }

        public ICollection<RatingsViewModel> JewelryRatings { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Jewel, IndexViewModel>()
            .ForMember(
                d => d.Images,
                opt => opt.MapFrom(x => x.Images.Select(t => t.ImageUrl)))
            .ForMember(
               d => d.Ratings,
               opt => opt.MapFrom(x => x.Ratings.Average(t => (double)t.Type)))
            .ForMember(
               d => d.JewelryRatings,
               opt => opt.MapFrom(x => x.Ratings));
        }
    }
}
