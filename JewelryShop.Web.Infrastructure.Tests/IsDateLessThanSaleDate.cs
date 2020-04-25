using JewelryShop.Web.Infrastructure.VilidationAttributes;
using JewelryShop.Web.ViewModels.Administration.Jewelry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace JewelryShop.Web.Infrastructure.Tests
{
    public class SaleDateLessThanAttributeTests
    {
     
        [Fact]
        public void IsDateLessThanSaleDate()
        {
            object value = new DateTime(2020, 5, 2);

            EditJewelViewModel model = new EditJewelViewModel()
            {
                SaleDate= (DateTime)value,
                SalePrice =10
            };

            var validationResults = new List<ValidationResult>();           
            
            var validationAttribute = new SaleDateLessThanAttribute("SalePrice");
            
            var context = new ValidationContext(model);
           
            var result = validationAttribute.GetValidationResult(value, context);

            Assert.True(result == ValidationResult.Success);

        }

        //Empty SalePrice
        [Fact]
        public void EmptyPriceDate()
        {
            DateTime date = DateTime.UtcNow.AddDays(-20);
           
            EditJewelViewModel model = new EditJewelViewModel()
            { 
                SaleDate = date
            };

            var validationResults = new List<ValidationResult>();
            var validationAttribute = new SaleDateLessThanAttribute("SalePrice");

            var context = new ValidationContext(model);
            var result = validationAttribute.GetValidationResult((object)date, context);

            Assert.True(result != ValidationResult.Success);
        }

        //Empty SalePrice
        [Fact]
        public void EmptySaleDate()
        {
            object date = null;
            EditJewelViewModel model = new EditJewelViewModel() 
            {
                SalePrice = 20 
            };

            var validationResults = new List<ValidationResult>();
            var validationAttribute = new SaleDateLessThanAttribute("SalePrice");

            var context = new ValidationContext(model);
            var result = validationAttribute.GetValidationResult((object)date, context);

            Assert.True(result == ValidationResult.Success);
        }

        [Fact]
        public void EmptyAllData()
        {
            object date = null;
            EditJewelViewModel model = new EditJewelViewModel() { };

            var validationResults = new List<ValidationResult>();
            var validationAttribute = new SaleDateLessThanAttribute("SalePrice");
            
            var context = new ValidationContext(model);
            var result = validationAttribute.GetValidationResult((object)date, context);

            Assert.True(result == ValidationResult.Success);
        }
    }
}
