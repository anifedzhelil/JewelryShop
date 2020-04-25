using JewelryShop.Data.Models;
using JewelryShop.Data.Models.Enums;
using JewelryShop.Services.Data;
using JewelryShop.Web.Controllers;
using JewelryShop.Web.ViewModels.ProductDetails;
using JewelryShop.Web.ViewModels.Ratings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JewelryShop.Web.Tests
{
    public class ProductDetailsControllerTests
    {
        [Fact]
        public void TestInputModelForIndexForm()
        {
            var mockService = new Mock<IJewelryService>();
            mockService.Setup(x => x.GetById<IndexViewModel>(5)).Returns(new IndexViewModel
            {
                Id = 5,
                Count = 2,
                JewelryRatings = null,
                Images = null,
                Name = "бижу",
                Description = "бижу",
                Ratings = 0,
            });
            var controller = new ProductDetailsController(mockService.Object);
            var result = controller.Index(5);
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = viewResult.Model as IndexViewModel;
            Assert.Equal(5, viewModel.Id);

            mockService.Verify(x => x.GetById<IndexViewModel>(5));
        }
    }

}
