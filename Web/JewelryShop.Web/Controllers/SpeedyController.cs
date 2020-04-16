namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using JewelryShop.Web.ViewModels.Speedy;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [ApiController]
    [Route("api/[controller]")]
    public class SpeedyController : Controller
    {
        private static readonly HttpClient Client = new HttpClient();

        [HttpPost]
        public OfficesViewModel GetOffices()
        {
            string url = "https://api.speedy.bg/v1/location/office/";

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = @"application/json; charset=utf-8";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                var bodyContent = new
                {
                    userName = "999151",
                    password = "2537621445",
                    language = "BG",
                    countryId = 100,
                };

                var json = JsonConvert.SerializeObject(bodyContent);

                stream.Write(json);
            }

            HttpWebResponse webresponse = request.GetResponse() as HttpWebResponse;

            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");

            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);

            string strResult = responseStream.ReadToEnd();
            responseStream.Close();
            webresponse.Close();

            dynamic result = JsonConvert.DeserializeObject(strResult);

            OfficesViewModel offices = new OfficesViewModel()
            {
                Addresses = new List<AddressViewModel>(),
            };

            foreach (var item in result.offices)
            {
                AddressViewModel adress = new AddressViewModel()
                {
                    Id = (int)item["id"],
                    FullAddress = (string)item["address"]["fullAddressString"],
                };
                offices.Addresses.Add(adress);
            }

            return offices;
        }
    }
}
