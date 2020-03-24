using JewelryShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace JewelryShop.Services.Data
{

    public class JewelryCategoriesService : IJewelryCategoriesService
    {
        public Dictionary<int, string> GetJewelryCategories()
        {
            Dictionary<int, string> dictJewelTypes = new Dictionary<int, string>();
            foreach (JewelryCategories.JewelryCategoriesEnum value in Enum.GetValues(typeof(JewelryCategories.JewelryCategoriesEnum)))
            {
                Type enumType = value.GetType();
                var enumValue = Enum.GetName(enumType, value);
                MemberInfo member = enumType.GetMember(enumValue)[0];

                var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                var outString = ((DisplayAttribute)attrs[0]).Name;

                if (((DisplayAttribute)attrs[0]).ResourceType != null)
                {
                    outString = ((DisplayAttribute)attrs[0]).GetName();
                }

                int intValue = (int)value;
                dictJewelTypes.Add(intValue, outString);
            }

            return dictJewelTypes;
        }
    }
}
