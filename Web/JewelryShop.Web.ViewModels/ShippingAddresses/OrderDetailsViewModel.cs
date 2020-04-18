namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using System;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class OrderDetailsViewModel : IMapFrom<OrderDetails>, IHaveCustomMappings
    {
        public string JewelName { get; set; }

        public int Quantity { get; set; }

        public decimal CurrentPrice { get; set; }

        public int OrderId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderDetails, OrderDetailsViewModel>()
                .ForMember(
                 d => d.CurrentPrice,
                 opt => opt.MapFrom(x => x.Jewel.SaleDate > DateTime.UtcNow ? x.Jewel.SalePrice : x.Jewel.Price));
        }
    }
}
