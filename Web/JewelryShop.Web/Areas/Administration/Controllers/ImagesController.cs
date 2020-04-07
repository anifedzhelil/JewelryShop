namespace JewelryShop.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.CloudinaryHelper;
    using JewelryShop.Web.ViewModels.Administration.Jewelry;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly Cloudinary cloudinary;
        private readonly IJewelryImagesService imageService;

        public ImagesController(Cloudinary cloudinary, IJewelryImagesService imageService)
        {
            this.cloudinary = cloudinary;
            this.imageService = imageService;
        }

        [HttpPost]
        public async Task<bool> DeleteImageAsync(ImageViewModel model)
        {
            var image = System.IO.Path.GetFileNameWithoutExtension(model.ImageUrl);
            this.imageService.DeleteImage(model.JewelId, model.ImageUrl);
            await CloudinaryExtention.DeleteImageAsync(this.cloudinary, image);

            return true;
        }
    }
}
