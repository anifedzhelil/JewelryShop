namespace JewelryShop.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class JewerlyDetailViewModel : IMapFrom<Jewel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public DateTime SaleDate { get; set; }

        public int Count { get; set; }

        public int Category { get; set; }

        public IEnumerable<string> Images { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Jewel, JewerlyDetailViewModel>()
            .ForMember(
                d => d.Images,
                opt => opt.MapFrom(x => x.Images.Select(t => t.ImageUrl)));
        }
    }
}
