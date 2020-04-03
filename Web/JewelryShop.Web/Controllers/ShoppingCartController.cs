namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;

    public class ShoppingCartController : BaseController
    {
        private const string GuestId = "guest_id";
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartController(IOrdersService ordersService, UserManager<ApplicationUser> userManager)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
           /* {
                OrdersDetails = this.ordersService.GetActiveGuestOrder<OrderDetailsIndexViewModel>(this.Request.Cookies[GuestId].ToString()),
            };*/

        
        var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                if (this.Request.Cookies[GuestId] != null)
                {
                    model = this.ordersService.GetActiveGuestOrder<IndexViewModel>(this.Request.Cookies[GuestId].ToString());
                }
            }
            else
            {
                model = this.ordersService.GetActiveOrder<IndexViewModel>(user.Id);
            }

            if (model.OrdersDetails != null)
            {
                return this.View(model);
            }
            else
            {
                return this.View("Empty");
            }
        }

       /* public IActionResult GetList()
        {
            using (var package = new ExcelPackage(new FileInfo("wwwroot/files/EcontLimitOfficeSchedule.xlsx")))
            {
               /* var firstSheet = package.Workbook.Worksheets["First Sheet"];
                Console.WriteLine("Sheet 1 Data");
                Console.WriteLine($"Cell A2 Value   : {firstSheet.Cells["A2"].Text}");
                Console.WriteLine($"Cell A2 Color   : {firstSheet.Cells["A2"].Style.Font.Color.LookupColor()}");
                Console.WriteLine($"Cell B2 Formula : {firstSheet.Cells["B2"].Formula}");
                Console.WriteLine($"Cell B2 Value   : {firstSheet.Cells["B2"].Text}");
                Console.WriteLine($"Cell B2 Border  : {firstSheet.Cells["B2"].Style.Border.Top.Style}");

                KeyValuePair<KeyValuePair<string, string> econtList =   
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...  
                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...  
                        object cellValue = workSheet.Cells[row, col].Text; // This got me the actual value I needed.  
                    }
                }

            }

            return this.View();
        }*/
    }
}