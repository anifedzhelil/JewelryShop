namespace JewelryShop.Web.ViewModels.ShoppingCart
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class IndexViewModel : IMapFrom<Order>
    {
        public IEnumerable<OrderDetailsIndexViewModel> OrdersDetails { get; set; }

        public int Id { get; set; }
    }
}
