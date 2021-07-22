using Google.Apis.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Administration;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class HomePageController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IOptions<CSrcSetting> _CSrcSetting;

        public HomePageController(DeliciousContext deliciousContext, IWebHostEnvironment hostEnvironment,IOptions<CSrcSetting> CSrcSetting) 
        {
            _deliciousContext = deliciousContext;
            _hostEnvironment = hostEnvironment;
            _CSrcSetting = CSrcSetting;
        }
        //首頁-----------------------------------------------------------
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = _CSrcSetting.Value.FigureSrc+HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
                ViewBag.USERNICKNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNICKNAME);
            }
            var photowall = _deliciousContext.TphotoWalls.Where(p => p.Display == true).Select(p => new {
                p,
                p.Category.CategoryName,
                p.Category.HtmlClassName,
                p.Member.Nickname
                });
            List<CPhotoWallViewModel> photowalls = new List<CPhotoWallViewModel>();
            foreach (var item in photowall)
            {
                CPhotoWallViewModel photoWall = new CPhotoWallViewModel()
                {
                    _photowall = item.p,
                    CategoryName = item.CategoryName,
                    HtmlClassName = item.HtmlClassName,
                    NickName = item.Nickname,
                };
                photowalls.Add(photoWall);
            }
           
            CHomePageViewModel models = new CHomePageViewModel();
            models.photowalls = photowalls;

            models.members = (_deliciousContext.Tmembers.Select(m => m)).OrderByDescending(m => m.PersonalRankScore).Take(3);
            models.showpics = _deliciousContext.TshowingPics.Where(t => t.ShowingPicsIsShowOrNot == true).Select(t => t);
            models.PhotoWallSrc = _CSrcSetting.Value.PhotoWallSrc;
            models.ShowPicsSrc = _CSrcSetting.Value.ShowPicsSrc;
            models.FigureSrc = _CSrcSetting.Value.FigureSrc;
            return View(models);
        }
        //登入註冊畫面-----------------------------------------------------------
        public IActionResult Login()
        {
            return View();
        }
        //Google登入-----------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> GoogleLogin(string id_token)
        {
            string msg = "ok";
            GoogleJsonWebSignature.Payload payload = null;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(id_token, new GoogleJsonWebSignature.ValidationSettings()
                {   
                    Audience = new List<string>() { CDictionary.Google_Client_id }//要驗證的client id，把自己申請的Client ID填進去
                });
            }
            catch (Google.Apis.Auth.InvalidJwtException ex)
            {
                return Content(ex.Message);
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                return Content(ex.Message);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            TGoogleRegister googleRegister = new TGoogleRegister()//google資料模型
            {
                user_id = payload.Subject,
                user_email = payload.Email,
                user_picture = payload.Picture,
            };

            if (msg == "ok" && payload != null)//如果google回傳成功開始判斷
            {
                var member = _deliciousContext.Tmembers.FirstOrDefault(t => t.Email == googleRegister.user_email);
                if (member != null && member.GoogleId != null)//如果已有此會員,也有googleid,就單純做登入
                {
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, member.AccountName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNICKNAME, member.Nickname);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO,_CSrcSetting.Value.FigureSrc+member.Figure);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, member.MemberId.ToString());
                    return Json(true);
                }
                else if (member != null && member.GoogleId == null)//如果此會員沒有googleid就做綁定,並更改email認證狀態
                {
                    member.GoogleId = googleRegister.user_id;
                    member.ConfirmedOrNotEmail = true;
                    try
                    {
                        _deliciousContext.SaveChanges();
                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        return Json(false);
                    }
                }
                else//此email沒使用過,註冊
                {
                    CGoogleRegister register = new CGoogleRegister(googleRegister, _deliciousContext,_hostEnvironment);
                    Tmember newMember = register.CreateMember();
                    if (newMember!=null)
                    {
                        HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, newMember.AccountName);
                        HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNICKNAME, newMember.Nickname);
                        HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, _CSrcSetting.Value.FigureSrc+newMember.Figure);
                        HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, newMember.MemberId.ToString());
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            return Json(false);
        }
        //註冊頁面-----------------------------------------------------------
        [HttpPost]
        public IActionResult Login(CLoginAndCreateViewModel model)
        {
            
            if (!string.IsNullOrEmpty(model.cLogin.AccountOrEmailOrCell) && !string.IsNullOrEmpty(model.cLogin.Password))
            {
                if (model.cLogin.LoginSuccess(model.cLogin, _deliciousContext))
                {
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, model.cLogin.member.AccountName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNICKNAME, model.cLogin.member.Nickname);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, _CSrcSetting.Value.FigureSrc+model.cLogin.member.Figure);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, model.cLogin.member.MemberId.ToString());
                    return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    ViewBag.ErrorMessage = "帳號或密碼錯誤";
                    return View();
                }
            }
            return View();
        }
        public bool CheckLogin()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //註冊表單送出-----------------------------------------------------------
        [HttpPost]
        public bool CreateMember(CLoginAndCreateViewModel model)
        {
            if (model != null)
            {
                model.cCreate.FillMemberData();
                if (model.cCreate.AddMember(model.cCreate._member,_deliciousContext))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }
        //熱門食譜-----------------------------------------------------------
        public JsonResult GetHotRecipe()
        {
            var recipe = from l in _deliciousContext.TlikeRecipes.AsEnumerable()
                         
                         group l by new
                         {
                             l.Recipe.RecipeName,
                             l.RecipeId,
                             l.Recipe.Member.Nickname,
                             l.Recipe.PostTime,
                             l.Recipe.PostTime.Year,
                             l.Recipe.PostTime.Month,
                             l.Recipe.DisVisible,
                             l.Recipe.DeleteOrNot,
                             l.Recipe.Picture,
                             l.Recipe.Views,
                             RecipeDescription = l.Recipe.RecipeDescription,
                             season = SeasonSort(l.Recipe.PostTime.Month)
                         } into g
                         where g.Key.season == "第三季" && g.Key.Year == DateTime.Now.Year && g.Key.DisVisible == false && g.Key.DeleteOrNot == false
                         select new
                         {
                             g.Key.RecipeName,
                             g.Key.Nickname,
                             g.Key.RecipeId,
                             g.Key.Picture,
                             g.Key.Views,
                             g.Key.Month,
                             g.Key.PostTime,
                             g.Key.RecipeDescription,
                             LikeCount = g.Count(),
                         };
            recipe = recipe.OrderByDescending(r => r.LikeCount);

            return Json(recipe);
        }
        private string SeasonSort(int month)
        {
            if (month <= 3) return "第一季";
            else if (month <= 6) return "第二季";
            else if (month <= 9) return "第三季";
            else return "第四季";
        }
        //食譜分類標籤-----------------------------------------------------------
        public JsonResult GetRecipeCategory()
        {
            var recipecategory = _deliciousContext.TrecipeCategories.Select(C => new { C.RecipeCategoryId, C.RecipeCategory });
            return Json(recipecategory);
        }
        //分類食譜-----------------------------------------------------------
        public JsonResult GetRecipe(int id)
        {
            var recipe = from r in _deliciousContext.Trecipes
                          let like = (from r2 in _deliciousContext.TlikeRecipes.AsEnumerable() where r2.RecipeId == r.RecipeId select r2).Count()
                          where r.RecipeCategoryId == id && r.DisVisible == false && r.DeleteOrNot == false
                          select new
                          {
                              r.RecipeId,
                              r.RecipeName,
                              RecipeDescription = r.RecipeDescription.Substring(0,40)+"...",
                              r.Picture,
                              r.Member.Nickname,
                              r.Views,
                              r.PostTime,
                              LikeCount = like
                          };
            return Json(recipe);
        }
        //登出-----------------------------------------------------------
        public string Logout()
        {
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERNAME);
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERPHOTO);
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERID);
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERNICKNAME);

            return "登出成功";
        }
        public IActionResult PhotoContribute()
        {
            CPhotoWallViewModel model = new CPhotoWallViewModel()
            {
                _categorys = _deliciousContext.TphotoWallCategories.Select(t => t),
                MemberId = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)),
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult PhotoContribute(CPhotoWallViewModel model)
        {
            model._categorys = _deliciousContext.TphotoWallCategories.Select(t => t);
            model.MemberId = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            if (model.InsertPhotoWall(model, _deliciousContext, _hostEnvironment))
            {
                ModelState.AddModelError("Error", "Success!!");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("Error", "Error!!");
                return View(model);
            }
        }
    }
}
