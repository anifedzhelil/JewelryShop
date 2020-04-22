namespace JewelryShop.Web.ViewModels.Administration.Jewelry
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

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsArchived { get; set; }

        public int Count { get; set; }

        public string ImageUr { get; set; }

        public int SoldCount { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Jewel, IndexJewelryViewModel>()
            .ForMember(
                d => d.ImageUr,
                opt => opt.MapFrom(x => x.Images.Select(t => t.ImageUrl).FirstOrDefault()))
            .ForMember(
                d => d.SoldCount,
                opt => opt.MapFrom(x => x.OrderDetails.Where(x => x.Price > 0).Sum(t => t.Quantity)))
            .ForMember(
                d => d.Rating,
                opt => opt.MapFrom(x => x.Ratings.Average(t => (double)t.Type)));
        }
    }
}
