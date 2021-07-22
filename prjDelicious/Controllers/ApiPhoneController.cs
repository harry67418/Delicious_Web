using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebChart.Models;

namespace prjDelicious.Controllers
{
   
    
    public class ApiPhoneController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly DeliciousContext _delicious;

        public ApiPhoneController(IWebHostEnvironment hostingEnvironment, DeliciousContext delicious)
        {
            _delicious = delicious;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult ImageInput(CImageInput _cImage)
        {
            //將檔案存到uploads資料夾中
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads"); //c:\......\uploads
            string FileName = Guid.NewGuid().ToString() + ".jpg";
            string filePath = Path.Combine(uploadsFolder, FileName);//c:\......\uploads\cats.jpg            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                _cImage.Photo.CopyTo(fileStream);
            }
            byte[] imgByte = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                _cImage.Photo.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            //string strbase64 = Convert.ToBase64String(imgByte);
            return Content($"{FileName}", "text/plain", System.Text.Encoding.UTF8);
        }
        public ActionResult GetImage(string fileName)
        {
            return File("~/uploads/" + fileName, "image/jpeg");
        }
        public IActionResult RecipeList() //食譜
        {
            var RecipeCategory = _delicious.Trecipes.Select(n => new
            {
                n.RecipeId,
                n.RecipeName,
                n.RecipeDescription,
                n.Picture
            });

            return Json(RecipeCategory);
        }
        public IActionResult RecipeStep(int id) //食譜
        {
            var RecipeCategory = _delicious.Tsteps.Where(n => n.RecipeId == id).OrderBy(n=>n.StepsNumber).Select(n => new
            {
                n.StepsNumber,
                n.Steps,
                n.Picture
            });

            return Json(RecipeCategory);
        }
        public IActionResult Shopitemlist()
        {

            //var deliciousContext = _delicious.TmerchandisePictures.Include(t => t.Ingredient).ThenInclude(t=>t.IngredientCategory).Where(t=>t.Ingredient.InStoreOrNot == true).Select(n => new
            //{
            //    n.IngredientId,
            //    n.Ingredient,
            //    n.Ingredient.IngredientUnit,
            //    n.Ingredient.Price,
            //    n.Ingredient.IngredientCategory.IngredientCategory,
            //    n.MerchandisePicture
            //}
            //) ;
            var deliciousContext = _delicious.Tingredients.Include(t => t.IngredientCategory).Where(t => t.InStoreOrNot == true).Select(n => new
            {
                n.IngredientId,
                n.Ingredient,
                n.IngredientUnit,
                n.Price,
                n.IngredientCategory.IngredientCategory,
                MerchandisePicture = _delicious.TmerchandisePictures.Where(z => z.IngredientId == n.IngredientId).Select(n => n.MerchandisePicture).FirstOrDefault()
            }
         );


            return Json(deliciousContext);
        }

        [HttpPost]
        public IActionResult AddDeliverInfo([FromBody] Torder torder)
        {
            if (torder.Reciever == "" || torder.Reciever == null) { return Content("empty"); }

            Torder info = new Torder();
            info = torder;
            info.MemberId = 51;
            info.OrderDate = DateTime.Now.ToLocalTime();
            info.OrderStatus = "待付款";
            info.PayMethod = "信用卡";
            info.RecieveMethod = "宅配到府";

            _delicious.Torders.Add(info);
            _delicious.SaveChanges();
            return Json(info);

        }

        [HttpPost]
        public IActionResult Addorder([FromBody] object oContext)
        {
            var cCartItemModel = JsonConvert.DeserializeObject<List<CCartItemModel>>(oContext.ToString());

            if (cCartItemModel[0].ingredient == "" || cCartItemModel[0].ingredient == null)
            {
                return Content("empty");
            }
            int Orderid = _delicious.Torders.Where(n => n.MemberId == 51).OrderByDescending(n => n.OrderId).Select(n => n.OrderId).First();
            if (_delicious.TorderDetails.Where(n => n.OrderiD == Orderid).Count() != 0)
            {
                return Content("很抱歉，訂單失敗，請稍後再嘗試");
            }

            foreach (var item in cCartItemModel)
            {
                TorderDetail detail = new TorderDetail();
                detail.OrderiD = Orderid;
                detail.IngredientId = Convert.ToInt32(item.Ingid);
                detail.InCartQuantity = Convert.ToInt32(item.CartQty);
                detail.Price = Convert.ToDecimal(item.unitprice.Replace("元", ""));
                detail.Discount = 0;
                _delicious.TorderDetails.Add(detail);
                _delicious.SaveChanges();
            }

            foreach (var item in cCartItemModel)
            {
                var p = _delicious.Tingredients.Where(n => n.IngredientId == Convert.ToInt32(item.Ingid)).Select(n => n).FirstOrDefault();
                p.AmountInStore = p.AmountInStore - Convert.ToInt32(item.CartQty);
                _delicious.SaveChanges();
            }

            return Content("完成");
        }
        [HttpPost]
        public IActionResult member_account_check([FromBody] Tmember _tmember)
        {
            if (_tmember.AccountName == "" || _tmember.AccountName == null ||
                _tmember.Password == "" || _tmember.Password == null) { return Content("empty"); }
            var p = _delicious.Tmembers.Where(n => n.AccountName == _tmember.AccountName && n.Password == _tmember.Password).Select(n => new { n.MemberId, n.Nickname}).FirstOrDefault();
            
           
            string member_result = JsonConvert.SerializeObject(p);

            if (p != null)
            {
                return Content(member_result);
            }
            else
            {
                return Content("account_no_found");
            }

        }
    }
}
