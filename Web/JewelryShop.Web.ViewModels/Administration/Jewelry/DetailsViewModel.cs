namespace JewelryShop.Web.ViewModels.Administration.Jewelry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.Ratings;

    public class DetailsViewModel : IMapFrom<Jewel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public DateTime SaleDate { get; set; }

        public int Count { get; set; }

        public CategoryType Category { get; set; }

        public IEnumerable<string> Images { get; set; }

        public double Ratings { get; set; }

        public int TypeCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Jewel, DetailsViewModel>()
            .ForMember(
                d => d.Images,
                opt => opt.MapFrom(x => x.Images.Select(t => t.ImageUrl)))
            .ForMember(
               d => d.Ratings,
               opt => opt.MapFrom(x => x.Ratings.Average(t => (double)t.Type)))
             .ForMember(
               d => d.TypeCount,
               opt => opt.MapFrom(x => x.Ratings.Count()));
        }
    }
}
