namespace JewelryShop.Web.ViewModels.ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class OrderDetailsIndexViewModel : IMapFrom<OrderDetails>, IHaveCustomMappings
    {
        public int JewelId { get; set; }

        public int JewelCount { get; set; }

        public string JewelName { get; set; }

        public decimal CurrentPrice { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderDetails, OrderDetailsIndexViewModel>()
               .ForMember(
                 d => d.ImageUrl,
                 opt => opt.MapFrom(x => x.Jewel.Images.Select(t => t.ImageUrl).FirstOrDefault()))
                .ForMember(
                 d => d.CurrentPrice,
                 opt => opt.MapFrom(x => x.Jewel.SaleDate > DateTime.UtcNow ? x.Jewel.SalePrice : x.Jewel.Price));
        }
    }
}
