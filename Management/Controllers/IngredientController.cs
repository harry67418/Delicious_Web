using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IngredientController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()//停用此個
        {
            CIngredientItemViewModel Model = new CIngredientItemViewModel();
            Model.CatIdList = categoriesfun();

            return View(Model);
        }

        public IActionResult conditions(CConditionsofIngredientViewModel conditions)
        {
            HttpContext.Session.SetString("conditionsofing", "");
            HttpContext.Session.SetString("conditionsofing", JsonConvert.SerializeObject(conditions));

            int s = 1;
            var items = _deliciousContext.Tingredients.Select(n =>
            new
            {
                n.IngredientId,
                n.Ingredient,
                n.IngredientCategoryId,
                n.IngredientUnit,
                n.Price,
                n.AmountInStore,
                n.MerchandiseDescription,
                n.InStoreOrNot,
                IngredientCategory = n.IngredientCategory.IngredientCategory

            });
            if (conditions.SelIngCat != 0)
                items = items.Where(n => n.IngredientCategoryId == conditions.SelIngCat);
            if (!string.IsNullOrEmpty(conditions.SelKeywords))
                items = items.Where(n => n.Ingredient.Contains(conditions.SelKeywords));
            if (conditions.SelMin != 0)
            {
                int a = conditions.SelMin;
                items = items.Where(n => n.Price >= a);
            }

            if (conditions.SelMax != 0)
            {
                int a = conditions.SelMin;
                items = items.Where(n => n.Price <= a);
            }
            if (conditions.SelSale == 0)
                items = items.Where(n => n.InStoreOrNot == false);
            else if (conditions.SelSale == 1)
                items = items.Where(n => n.InStoreOrNot == true);
            if (items == null)
            {
                return NoContent();
            }
            HttpContext.Session.SetString("Ings", "");
            #region sessions
            CIngredientInfoViewModel Model = new CIngredientInfoViewModel();
            Model.CatIdList = categoriesfun();
            foreach (var item in items)
            {
                CIngredientItemViewModel one = new CIngredientItemViewModel();
                one.IngredientId = item.IngredientId;
                one.Ingredient = item.Ingredient;
                one.IngredientCategoryID = item.IngredientCategoryId;
                one.IngredientCategory = item.IngredientCategory;
                one.IngredientUnit = item.IngredientUnit;
                one.Price = item.Price;
                one.AmountInStore = item.AmountInStore;
                one.MerchandiseDescription = item.MerchandiseDescription;
                one.InStoreOrNot = item.InStoreOrNot;
                Model.IdList.Add(one);
                HttpContext.Session.SetString("name", one.Ingredient);
            }
            HttpContext.Session.SetString("Ings", JsonConvert.SerializeObject(Model));
            #endregion
            return Json(items);
        }
        public CIngredientInfoViewModel setsession(string SelKeywords, int? SelIngCat, int? SelSale)
        {
            CConditionsofIngredientViewModel conditions = new CConditionsofIngredientViewModel();
            conditions.SelKeywords = SelKeywords;
            conditions.SelIngCat = (int)SelIngCat;
            conditions.SelSale = (int)SelSale;
            HttpContext.Session.SetString("conditions", JsonConvert.SerializeObject(conditions));
            var items = _deliciousContext.Tingredients.Select(n =>
           new
           {
               n.IngredientId,
               n.Ingredient,
               n.IngredientCategoryId,
               n.IngredientUnit,
               n.Price,
               n.AmountInStore,
               n.MerchandiseDescription,
               n.InStoreOrNot,
               IngredientCategory = n.IngredientCategory.IngredientCategory

           });
            if (!String.IsNullOrEmpty(SelKeywords))
            {
                items = items.Where(n => n.Ingredient.Contains(SelKeywords));

            }
            if (SelIngCat != 0)
            {
                items = items.Where(n => n.IngredientCategoryId == SelIngCat);

            }

            if (SelSale != 0)
            {
                if (SelSale == 1)
                    items = items.Where(n => n.InStoreOrNot == true);
                else if (SelSale == 2)
                    items = items.Where(n => n.InStoreOrNot == false);

            }
            #region sessions

            CIngredientInfoViewModel Model = new CIngredientInfoViewModel();
            Model.CatIdList = categoriesfun();
            foreach (var item in items)
            {
                CIngredientItemViewModel one = new CIngredientItemViewModel();
                one.IngredientId = item.IngredientId;
                one.Ingredient = item.Ingredient;
                one.IngredientCategoryID = item.IngredientCategoryId;
                one.IngredientCategory = item.IngredientCategory;
                one.IngredientUnit = item.IngredientUnit;
                one.Price = item.Price;
                one.AmountInStore = item.AmountInStore;
                one.MerchandiseDescription = item.MerchandiseDescription;
                one.InStoreOrNot = item.InStoreOrNot;
                Model.IdList.Add(one);

            }
            return Model;
            #endregion

        }
        public CIngredientInfoViewModel setsession()
        {
            var items = _deliciousContext.Tingredients.Select(n =>
           new
           {
               n.IngredientId,
               n.Ingredient,
               n.IngredientCategoryId,
               n.IngredientUnit,
               n.Price,
               n.AmountInStore,
               n.MerchandiseDescription,
               n.InStoreOrNot,
               IngredientCategory = n.IngredientCategory.IngredientCategory

           });
            HttpContext.Session.Remove("conditions");
            #region sessions
            CConditionsofIngredientViewModel conditions = new CConditionsofIngredientViewModel();
            CIngredientInfoViewModel Model = new CIngredientInfoViewModel();
            Model.CatIdList = categoriesfun();
            foreach (var item in items)
            {
                CIngredientItemViewModel one = new CIngredientItemViewModel();
                one.IngredientId = item.IngredientId;
                one.Ingredient = item.Ingredient;
                one.IngredientCategoryID = item.IngredientCategoryId;
                one.IngredientCategory = item.IngredientCategory;
                one.IngredientUnit = item.IngredientUnit;
                one.Price = item.Price;
                one.AmountInStore = item.AmountInStore;
                one.MerchandiseDescription = item.MerchandiseDescription;
                one.InStoreOrNot = item.InStoreOrNot;
                Model.IdList.Add(one);

            }
            return Model;
            #endregion

        }


        public IActionResult SessionDemoSend(int pg, int refresh, string SelKeywords, int SelIngCat, int SelSale)
        {

            CConditionsofIngredientViewModel oldcondition = new CConditionsofIngredientViewModel();
            CIngredientInfoViewModel Model = new CIngredientInfoViewModel();
            if (refresh == 1) { Model = setsession(); }
            else
            {
                CConditionsofIngredientViewModel newconditions = new CConditionsofIngredientViewModel();
                newconditions.SelKeywords = SelKeywords;
                newconditions.SelIngCat = (int)SelIngCat;
                newconditions.SelSale = (int)SelSale;

                if (string.IsNullOrEmpty(newconditions.SelKeywords) && newconditions.SelIngCat == 0 && newconditions.SelSale == 0)
                {

                    if (HttpContext.Session.GetString("conditions") != null)
                    {
                        oldcondition = JsonConvert.DeserializeObject<CConditionsofIngredientViewModel>(HttpContext.Session.GetString("conditions"));
                        newconditions = oldcondition;
                        Model = setsession(newconditions.SelKeywords, newconditions.SelIngCat, newconditions.SelSale);
                    }
                    else
                    {
                        Model = setsession();
                    }

                }
                else
                {
                    Model = setsession(SelKeywords, SelIngCat, SelSale);
                }

            }

            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int resCount = Model.IdList.Count();
            CPager pager = new CPager(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Model.IdList.Skip(recSkip).Take(pager.Pagesize);
            this.ViewBag.Pager = pager;
            this.ViewBag.CatList = categoriesfun();
            return View(data);
        }

        public List<CIngredientCategoryViewModel> categoriesfun()
        {
            List<CIngredientCategoryViewModel> list = new List<CIngredientCategoryViewModel>();
            var categories = _deliciousContext.TingredientCategories.Select(n => n);
            foreach (var item in categories)
            {
                CIngredientCategoryViewModel copier = new CIngredientCategoryViewModel();
                copier.IngredientCategoryId = item.IngredientCategoryId;
                copier.IngredientCategory = item.IngredientCategory;
                list.Add(copier);
            }
            return list;//傳食材種類
        }

        public IActionResult Edit(int IngID)
        {

            CIngredientItemViewModel Model = new CIngredientItemViewModel();
            var q = _deliciousContext.Tingredients.FirstOrDefault(n => n.IngredientId == IngID);
            Model.IngredientId = q.IngredientId;
            Model.Ingredient = q.Ingredient;
            Model.IngredientCategoryID = q.IngredientCategoryId;
            Model.IngredientUnit = q.IngredientUnit;
            Model.Price = q.Price;
            Model.AmountInStore = q.AmountInStore;
            Model.MerchandiseDescription = q.MerchandiseDescription;
            Model.InStoreOrNot = q.InStoreOrNot;
            Model.CatIdList = categoriesfun();
            var p = _deliciousContext.TmerchandisePictures.Where(n => n.IngredientId == IngID).Select(n => n.MerchandisePicture);
            string fileName = p.FirstOrDefault();
            Model.MerchadisePicture = fileName;

            return View(Model);
        }

        [HttpPost]
        public IActionResult Edit(CIngredientItemViewModel Model)
        {
            //DeliciousContext dbcontext = new DeliciousContext();
            var q = _deliciousContext.Tingredients.FirstOrDefault(n => n.IngredientId == Model.IngredientId);
            q.IngredientId = Model.IngredientId;
            q.Ingredient = Model.Ingredient;
            q.IngredientCategoryId = Model.IngredientCategoryID;
            q.IngredientUnit = Model.IngredientUnit;
            q.Price = Model.Price;
            q.AmountInStore = Model.AmountInStore;
            q.MerchandiseDescription = Model.MerchandiseDescription;
            if (Model.InStoreOrNot)
                q.InStoreOrNot = true;
            else
                q.InStoreOrNot = false;

            _deliciousContext.SaveChanges();
            if (Model.FormfilePic != null)
            {
                string filename = "Ing" + Model.IngredientId.ToString() + ".jpg";
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img", "IngredientPic");
                string filePath = Path.Combine(uploadFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Model.FormfilePic.CopyTo(fileStream);
                }
                SavePic(Model.IngredientId, filename);
            }
            return RedirectToAction("SessionDemoSend");
        }

        public IActionResult Create()
        {
            CIngredientItemViewModel Model = new CIngredientItemViewModel();
            Model.CatIdList = categoriesfun();
            return View(Model);
        }

        [HttpPost]
        public IActionResult Create(CIngredientItemViewModel Model)
        {


            Tingredient ing = new Tingredient();
            ing.Ingredient = Model.Ingredient;
            ing.IngredientCategoryId = Model.IngredientCategoryID;
            ing.IngredientUnit = Model.IngredientUnit;
            ing.Price = Model.Price;
            ing.AmountInStore = Model.AmountInStore;
            ing.MerchandiseDescription = Model.MerchandiseDescription;
            ing.InStoreOrNot = Model.InStoreOrNot;


            _deliciousContext.Tingredients.Add(ing);
            _deliciousContext.SaveChanges();
            if (Model.FormfilePic != null)
            {
                string filename = "Ing" + Model.IngredientId.ToString() + ".jpg";
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img", "IngredientPic");
                string filePath = Path.Combine(uploadFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Model.FormfilePic.CopyTo(fileStream);
                }

                var q = _deliciousContext.Tingredients.OrderByDescending(n => n.IngredientId).Select(n => n.IngredientId).FirstOrDefault();
                SavePic(q, filename);
            }
            return RedirectToAction("SessionDemoSend");
        }

        public void SavePic(int IngId, string filename)
        {
            TmerchandisePicture pic = new TmerchandisePicture();
            pic.MerchandisePicture = filename;
            pic.IngredientId = IngId;
            _deliciousContext.TmerchandisePictures.Add(pic);
            _deliciousContext.SaveChanges();

        }

        public string Delete(int IngID)//新增販售關聯計算
        {
            var q = _deliciousContext.Tingredients.FirstOrDefault(n => n.IngredientId == IngID);
            var usedcount = _deliciousContext.TingredientRecords.Where(n => n.IngredientId == IngID).Select(n => n);
            var orderedcount = _deliciousContext.TorderDetails.Where(n => n.IngredientId == IngID).Select(n => n);
            var ingpics = _deliciousContext.TmerchandisePictures.Where(n => n.IngredientId == IngID).Select(n => n);
            if (usedcount.Count() == 0 && orderedcount.Count() == 0)
            {
                if (q != null)
                {
                    foreach (var items in ingpics)
                    {
                        _deliciousContext.Remove(items);
                    }
                    _deliciousContext.Remove(q);
                    _deliciousContext.SaveChanges();
                }
                return "刪除成功";
            }
            else
            {
                return "已被使用，無法刪除。";
            }
        }
        public IActionResult Ingdetail(int IngID)
        {
            var datas = _deliciousContext.TorderDetails.Where(n => n.IngredientId == IngID&&n.Order.OrderStatus!="已取消" ).OrderByDescending(n=>n.Order.OrderDate).Select(n => new
            {
                n.OrderiD,
                n.InCartQuantity,
                OrderDate= n.Order.OrderDate,
                n.Price
            });

            return Json(datas);
        }
        public IActionResult Recdetail(int IngID)
        {
            var datas = _deliciousContext.TingredientRecords.Where(n => n.IngredientId == IngID).OrderByDescending(n=>n.Recipe.PostTime).Select(n => new
            {
                n.RecipeId,
                RecipeName=n.Recipe.RecipeName.ToString(),
                PostTime=n.Recipe.PostTime,
               
            });

            return Json(datas);
        }
    }
    }
