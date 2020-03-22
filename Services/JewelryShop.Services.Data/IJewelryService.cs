namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IJewelryService
    {
        IEnumerable<T> GetAll<T>(int? count = null);
    }
}
