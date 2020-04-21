﻿namespace JewelryShop.Web.ViewModels.UserOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;

    public class IndexItemViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime CompleteDate { get; set; }

        public int OrdersDetailsCount { get; set; }

        public OrderStatusType Status { get; set; }

        public decimal TotalSum { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, IndexItemViewModel>()
                  .ForMember(
                    d => d.TotalSum,
                    opt => opt.MapFrom(x => x.OrdersDetails.Sum(t => t.Price) + x.ShippingPrice));
        }
    }
}