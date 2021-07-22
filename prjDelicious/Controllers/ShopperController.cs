using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class ShopperController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IOptions<CSrcSetting> _CSrcSetting;

        public ShopperController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext, IOptions<CSrcSetting> CSrcSetting)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
            _CSrcSetting = CSrcSetting;
        }
        public IActionResult MerchadiseDetail(int Ingid)
        {//商品明細
            var ing = _deliciousContext.Tingredients.Where(n => n.IngredientId == Ingid && n.InStoreOrNot).Select(n => new { n, n.TmerchandisePictures, n.IngredientCategory }).First();
            if (ing == null) { return RedirectToAction("Index"); }
            CShopperInfoItemViewModel oneitem = new CShopperInfoItemViewModel()
            {
                IngredientId = Ingid,
                Ingredient = ing.n.Ingredient,
                AmountInStore = ing.n.AmountInStore,
                MerchandiseDescription = ing.n.MerchandiseDescription,
                Price = ing.n.Price,
                IngredientUnit = ing.n.IngredientUnit,
                Category = ing.IngredientCategory.IngredientCategory,
                CategoryID = ing.IngredientCategory.IngredientCategoryId

            };
            var pics = _deliciousContext.TmerchandisePictures.Where(n => n.IngredientId == Ingid).OrderByDescending(n => n.MerchandisePicId).Select(n => n);
            if (pics.Count() != 0)
            {
                foreach (var pic in pics)
                {
                    string srcpath = _CSrcSetting.Value.IngredientSrc + pic.MerchandisePicture;

                    oneitem.PicList.Add(srcpath);
                }
            }

            var relative = _deliciousContext.Tingredients.Where(n => n.InStoreOrNot && n.IngredientCategoryId == oneitem.CategoryID&&n.IngredientId!=Ingid).OrderByDescending(n => n.AmountInStore).Select(n => new { n }).Take(4);
            foreach (var item in relative)
            {
                
                CCartItemViewModel one = new CCartItemViewModel();
                one.Ingid = item.n.IngredientId;
                one.ingredient = item.n.Ingredient;
                one.unitprice = item.n.Price;
                one.amountInStore = item.n.AmountInStore;
               
                oneitem.Relativelist.Add(one);
            }
            foreach (var item in oneitem.Relativelist)
            {
                var picname = _deliciousContext.TmerchandisePictures.OrderByDescending(n=>n.MerchandisePicId).Where(n => n.IngredientId == item.Ingid).Select(n => n.MerchandisePicture).FirstOrDefault();
                if (picname != null)
                {
                    
                        string srcpath = _CSrcSetting.Value.IngredientSrc + picname;

                       item.ingpicsrc = srcpath;
                    
                }

            }

            List<TingredientCategory> catlist = catebuttons();
            ViewBag.catlist = catlist;


            return View(oneitem);
        }

        private List<TingredientCategory> catebuttons()
        {
            var cat = _deliciousContext.Tingredients.Where(n=>n.InStoreOrNot==true).Select(n =>new { n.IngredientCategoryId,n.IngredientCategory.IngredientCategory }).Distinct();
            List<TingredientCategory> catlist = new List<TingredientCategory>();
            foreach (var item in cat)
            {
                TingredientCategory cate = new TingredientCategory();
                cate.IngredientCategory = item.IngredientCategory;
                cate.IngredientCategoryId = item.IngredientCategoryId;
                catlist.Add(cate);
            }

            return catlist;
        }

        public IActionResult Categorydata(int CatID)
        {//各類商品展示
            CShopperInfoViewModel shopperlist = new CShopperInfoViewModel();
            shopperlist = Getdata(CatID);
            List<TingredientCategory> catlist = catebuttons();
            ViewBag.catlist = catlist;

            return View(shopperlist);
        }
        public CShopperInfoViewModel Getdata(int CatID) 
        {//儲存商品資訊
           
            var merchadises = _deliciousContext.Tingredients.Where(n => n.InStoreOrNot == true&&n.AmountInStore>0).OrderByDescending(n=>n.AmountInStore).Select(n => new {
                IngredientId = n.IngredientId,
                Ingredient = n.Ingredient,
                AmountInStore = n.AmountInStore,
                MerchandiseDescription = n.MerchandiseDescription,
                Price = n.Price,
                IngredientUnit = n.IngredientUnit,
                CategoryID=n.IngredientCategoryId,
                Category = n.IngredientCategory.IngredientCategory

            });
            if (CatID != 0)
            {
                merchadises = merchadises.Where(n => n.CategoryID == CatID);
            }
            else
            {
                merchadises = merchadises.Take(12);
            }
            CShopperInfoViewModel shopperlist = new CShopperInfoViewModel();
            foreach (var item in merchadises)
            {
                CShopperInfoItemViewModel shopperInfo = new CShopperInfoItemViewModel();
                shopperInfo.IngredientId = item.IngredientId;
                shopperInfo.Ingredient = item.Ingredient;
                shopperInfo.AmountInStore = item.AmountInStore;
                shopperInfo.MerchandiseDescription = item.MerchandiseDescription;
                shopperInfo.Price = item.Price;
                shopperInfo.IngredientUnit = item.IngredientUnit;
                shopperInfo.Category = item.Category;
                shopperInfo.CategoryID = item.CategoryID;
                shopperlist.ListofMerchadises.Add(shopperInfo);

            }
            foreach (var merchadise in shopperlist.ListofMerchadises)
            {
                var pics = _deliciousContext.TmerchandisePictures.Where(n => n.IngredientId == merchadise.IngredientId).Select(n => n);
                if (pics.Count() != 0)
                {
                    foreach (var pic in pics)
                    {
                        string srcpath = _CSrcSetting.Value.IngredientSrc+pic.MerchandisePicture;

                        merchadise.PicList.Add(srcpath);
                    }
                }

            }
            return shopperlist;

        }
        public IActionResult Index()
        {//商品展示
            int CatID = 0;
            var cat = _deliciousContext.TingredientCategories.OrderByDescending(n=>n.Tingredients.Count()).Select(n => n);
            List<TingredientCategory> catlist = catebuttons();
            ViewBag.catlist = catlist;
            CShopperInfoViewModel shopperlist = new CShopperInfoViewModel();
            shopperlist = Getdata(CatID);
            
            return View(shopperlist);
        }


        public IActionResult Checkout() 
        {
            if (HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID) != null)
            {
                string memberid = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
                List<CCartItemViewModel> cartlist = new List<CCartItemViewModel>();

                int member_id = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
                if (HttpContext.Session.GetString("Cart") != null)
                { cartlist = JsonConvert.DeserializeObject<List<CCartItemViewModel>>(HttpContext.Session.GetString("Cart")); }
                var q = _deliciousContext.Tdistincts.Select(n => n.DeliveryCounty).Distinct();
                List<string> county = new List<string>();
               foreach (var item in q)
                {
                    county.Add(item);
                }
                ViewBag.county = county;
                ViewBag.member = _deliciousContext.Tmembers.Where(n => n.MemberId == Convert.ToInt32(memberid)).Select(n => n.MemberName).First();
                ViewBag.cellphone = _deliciousContext.Tmembers.Where(n => n.MemberId == Convert.ToInt32(memberid)).Select(n => n.CellNumber).First();
                ViewBag.memberid = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
                return View(cartlist);
            }
            else
            {
               return RedirectToAction("Login","HomePage");  
            }
           
            
           
        }
        public List<string> district(string county)
        {
            List<string> districtlist = new List<string>();
            var q = _deliciousContext.Tdistincts.Where(n => n.DeliveryCounty == county).Select(n => n.DeliveryDistinct);
            foreach (var item in q)
            {
                districtlist.Add(item);
            }

            return districtlist;
        }
        public string FinishChecking(Torder torder) 
        {
            if (torder.PhoneNumber == null || torder.Reciever == null || torder.DeliveryAddress == null)
            {
                return "資料未填寫確實";
            }
            else
            {
                if (HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID) != null)
                {
                    int Memberid = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
                    List<CCartItemViewModel> list = JsonConvert.DeserializeObject<List<CCartItemViewModel>>(HttpContext.Session.GetString("Cart"));

                    foreach (var item in list)
                    {
                        var amountinstore = _deliciousContext.Tingredients.First(n => n.IngredientId == item.Ingid).AmountInStore;
                        if (amountinstore < item.CartQty)
                        {
                            return "很抱歉，目前商品庫存量不足，請減少購買量";
                        }
                    }

                    Torder info = new Torder();
                    info = torder;
                    info.MemberId = Memberid;
                    info.OrderDate = DateTime.Now.ToLocalTime();
                    info.OrderStatus = "待付款";
                    info.PayMethod = "信用卡";
                    info.RecieveMethod = "宅配到府";

                    _deliciousContext.Torders.Add(torder);
                    _deliciousContext.SaveChanges();
                    int Orderid = _deliciousContext.Torders.Where(n => n.MemberId == Memberid).OrderByDescending(n => n.OrderId).Select(n => n.OrderId).First();
                    if (_deliciousContext.TorderDetails.Where(n => n.OrderiD == Orderid).Count() != 0)
                    {
                        return "很抱歉，訂單失敗，請稍後再嘗試";
                    }


                    foreach (var item in list)
                    {
                        TorderDetail detail = new TorderDetail();
                        detail.OrderiD = Orderid;
                        detail.IngredientId = item.Ingid;
                        detail.InCartQuantity = item.CartQty;
                        detail.Price = item.unitprice;
                        detail.Discount = 0;
                        _deliciousContext.TorderDetails.Add(detail);
                        _deliciousContext.SaveChanges();
                    }
                    HttpContext.Session.Remove("Cart");
                    foreach (var item in list)
                    {
                        var p = _deliciousContext.Tingredients.Where(n => n.IngredientId == item.Ingid).Select(n => n).FirstOrDefault();
                        p.AmountInStore = p.AmountInStore - item.CartQty;
                        _deliciousContext.SaveChanges();
                    }
                    string memberid = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
                    return "訂購成功,請盡快付款";
                }
                return "訂購失敗,請聯絡客服";
            }
        }
       
    }
}
