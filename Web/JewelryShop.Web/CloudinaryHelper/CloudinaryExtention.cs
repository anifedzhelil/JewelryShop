namespace JewelryShop.Web.CloudinaryHelper
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryExtention
    {
        public static async Task<List<string>> UploadAsync(Cloudinary cloudinary, ICollection<IFormFile> files)
        {
            List<string> list = new List<string>();

            foreach (var file in files)
            {
                byte[] destionationImage;

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    destionationImage = memoryStream.ToArray();
                }

                using (var destionationStream = new MemoryStream(destionationImage))
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, destionationStream),
                    };
                    var res = await cloudinary.UploadAsync(uploadParams);

                    if (res.Error == null)
                    {
                        list.Add(res.Uri.ToString());
                    }
                }
            }

            return list;
        }

        public static async Task DeleteImageAsync(Cloudinary cloudinary, string imageUrl)
        {
            var delParams = new DeletionParams(imageUrl)
            {
                PublicId = imageUrl,
                ResourceType = ResourceType.Image,
            };

            var result = await cloudinary.DestroyAsync(delParams);
        }
    }
}
