namespace JewelryShop.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels;

    public class AutoMapperMappings : IDisposable
    {
        public AutoMapperMappings()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
