namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.Ratings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService ratingsService;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingsController(IRatingService ratingsService, UserManager<ApplicationUser> userManager)
        {
            this.ratingsService = ratingsService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RatingResponseModel>> Post(CreateRatingViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.ratingsService.RateAsync(model.JewelId, userId, model.Rating, model.Review);
            var ratings = this.ratingsService.GetAvarageRating(model.JewelId, userId);

            return new RatingResponseModel { AvarageRating = ratings };
        }
    }
}
