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
        public int JewelId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int Count { get; set; }

        public string ImageUr { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
                configuration.CreateMap<Jewel, IndexJewelryViewModel>()
                .ForMember(
                    d => d.ImageUr,
                    opt => opt.MapFrom(x => x.Images.Select(t => t.ImageUrl).FirstOrDefault()));
        }
    }
}
