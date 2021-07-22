using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class ForRecipeController : Controller
    {
        private readonly DeliciousContext _delicious;
        private readonly IWebHostEnvironment _host;
        private readonly IOptions<CSrcSetting> _CSrcSetting;
        private readonly IOptions<CEmailSetting> _CEmailSetting;
        CScoreCalculation ScoreCalculation = null;
        public ForRecipeController(DeliciousContext deliciousContext, IWebHostEnvironment hostEnvironment, IOptions<CSrcSetting> CSrcSetting, IOptions<CEmailSetting> CEmailSetting)
        {
            _delicious = deliciousContext;
            _host = hostEnvironment;
            _CSrcSetting = CSrcSetting;
            _CEmailSetting = CEmailSetting;
            ScoreCalculation = new CScoreCalculation(_delicious);
        }

        public IActionResult Recipe(int id) //食譜頁面
        {
            var Recipe = _delicious.Trecipes.Where(a => a.RecipeId == id).Select(a => new
            {
                recipePicture = a.Picture,
                recipeName = a.RecipeName,
                recipeDescription = a.RecipeDescription,
                recipeCategory = _delicious.TrecipeCategories.Where(j => j.RecipeCategoryId == a.RecipeCategoryId).Select(j => j.RecipeCategory).FirstOrDefault(),
                recipeVideo = a.Video,
                recipelikeCount = _delicious.TlikeRecipes.Where(p => p.RecipeId == id).Count(),
                _ingredientRecord = _delicious.TingredientRecords.Where(b => b.RecipeId == id).Select(b => new
                {
                    b.IngredientRecordId,
                    ingredient = _delicious.Tingredients.Where(c => c.IngredientId == b.IngredientId).Select(c => c.Ingredient).FirstOrDefault(),
                    ingredientAmountForUse = b.IngredientAmountForUse,
                    unt = b.Unt
                }).OrderBy(b => b.IngredientRecordId).ToList(),
                MemberId = a.MemberId,
                Nickname = _delicious.Tmembers.Where(f => f.MemberId == a.MemberId).Select(f => f.Nickname).FirstOrDefault(),
                MemberPicture = _delicious.Tmembers.Where(h => h.MemberId == a.MemberId).Select(x => x.Figure).FirstOrDefault(),
                MemberFollow = _delicious.TmemberFollows.Where(q => q.FollowMemberId == a.MemberId).Select(w => w.FollowMemberId).Count(),
                postTime = a.PostTime,
                forHowmany = a.ForHowMany,
                timeNeed = a.TimeNeed / 60,
                views = a.Views,
                _step = _delicious.Tsteps.Where(g => g.RecipeId == id).Select(g => new
                {
                    stepPicture = g.Picture,
                    stepNumber = g.StepsNumber,
                    step = g.Steps
                }).OrderBy(g => g.stepNumber).ToList()
            }).FirstOrDefault();

            try
            {
                CRecipeViewModel cRecipe = new CRecipeViewModel();
                cRecipe.recipeId = id;
                cRecipe.recipePicture = Recipe.recipePicture;
                cRecipe.recipeName = Recipe.recipeName;
                cRecipe.recipeDescription = Recipe.recipeDescription;
                cRecipe.recipeCategory = Recipe.recipeCategory;
                cRecipe.recipeVideo = Recipe.recipeVideo;
                cRecipe.Nickname = Recipe.Nickname;
                cRecipe.postTime = Recipe.postTime;
                cRecipe.forHowmany = Recipe.forHowmany;
                cRecipe.timeNeed = Recipe.timeNeed;
                cRecipe.views = Recipe.views + 1;
                cRecipe.MemberId = Recipe.MemberId;
                cRecipe.MemberPicture = Recipe.MemberPicture;
                cRecipe.recipelikeCount = Recipe.recipelikeCount;
                cRecipe.followerCount = Recipe.MemberFollow;
                cRecipe.followeds = _delicious.TmemberFollows.Where(t => t.FollowMemberId == Recipe.MemberId).Select(t => t).ToList();//被誰追蹤
                foreach (var item in Recipe._ingredientRecord)
                {
                    CIngredientRecordViewModel cIngredientRecord = new CIngredientRecordViewModel();
                    cIngredientRecord.ingredient = item.ingredient;
                    cIngredientRecord.ingredientAmountForUse = item.ingredientAmountForUse;
                    cIngredientRecord.unt = item.unt;
                    cRecipe.list_ingredientRecords.Add(cIngredientRecord);
                }
                foreach (var item in Recipe._step)
                {
                    CRecipeStepViewModel cRecipeStep = new CRecipeStepViewModel();
                    cRecipeStep.stepPicture = item.stepPicture;
                    cRecipeStep.stepNumber = item.stepNumber;
                    cRecipeStep.step = item.step;
                    cRecipe.list_step.Add(cRecipeStep);
                }

                var _comments = _delicious.TcommentSections.Where(t => t.RecipeId == id).Select(t => new
                {
                    t,
                    t.Member,
                    likeCount = _delicious.TlikeComments.Where(t2 => t2.CommentId == t.CommentId).Count()
                }).ToList();
                foreach (var item in _comments)
                {
                    CCommentViewModel ccomments = new CCommentViewModel()
                    {
                        _comment = item.t,
                        _member = item.Member,
                        likeCouint = item.likeCount
                    };
                    cRecipe.list_comment.Add(ccomments);
                }
                //留言區
                if (HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID) != null)
                {
                    var _member = _delicious.Tmembers.Where(t => t.MemberId.ToString() == HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)).FirstOrDefault();
                    cRecipe.member = _member;
                    cRecipe.list_likecomment = _delicious.TlikeComments.Where(t => t.MemberId == _member.MemberId).ToList();
                    cRecipe.list_Folders = _delicious.TcollectionFolders.Where(t => t.MemberId == _member.MemberId).ToList();
                }
                else
                {
                    cRecipe.member = null;
                    cRecipe.list_likecomment = null;
                    cRecipe.list_Folders = null;
                }

                cRecipe.CommentPicsSrc = _CSrcSetting.Value.CommentPicsSrc;
                cRecipe.FigureSrc = _CSrcSetting.Value.FigureSrc;

                var _Browse = _delicious.Trecipes.Where(r => r.RecipeId == id).Select(r => r).FirstOrDefault();
                _Browse.Views = _Browse.Views + 1;
                _delicious.SaveChanges();
                return View(cRecipe);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "HomePage");
            }

        }

        //[Authorize]
        public IActionResult Add_Recipe(CRecipeListViewModel r) //新增食譜
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add_Recipe(CRecipeViewModel r)  //新增食譜((POST部分)
        {
            try
            {
                Trecipe recipe = new Trecipe();
                recipe.RecipeName = r.recipeName.ToString();
                recipe.RecipeCategoryId = Convert.ToInt32(r.recipeCategoryId);
                recipe.RecipeDescription = r.recipeDescription.ToString();
                recipe.PostTime = DateTime.Now.ToLocalTime();
                recipe.ForHowMany = Convert.ToInt32(r.forHowmany);
                recipe.TimeNeed = Convert.ToInt32(r.timeNeed * 60);
                recipe.MemberId = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
                if (r.recipeVideo != null)
                {
                    recipe.Video = r.recipeVideo.ToString();
                }
                recipe.Views = 0;
                recipe.DeleteOrNot = false;
                recipe.DisVisible = false;
                if (r.recipephoto != null)
                {
                    string filename = $"{HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)}_{r.recipeName}.jpg";
                    string uploadFolder = Path.Combine(_host.WebRootPath, "assets", "img", "recipe");
                    string filePath = Path.Combine(uploadFolder, filename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        recipe.Picture = @$"/assets/img/recipe/{filename}";
                        r.recipephoto.CopyTo(fileStream);
                    }
                }
                _delicious.Trecipes.Add(recipe);
                _delicious.SaveChanges();

                var ID = _delicious.Trecipes.OrderByDescending(n => n.RecipeId).Where(n => n.MemberId == Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID))).Select(n => n.RecipeId).FirstOrDefault();
                if (r.list_ingredientRecords != null)
                {
                    if (ingredientRecord_Check(r.list_ingredientRecords))
                    {
                        foreach (var item in r.list_ingredientRecords)
                        {
                            if (item.ingredientCategoryId == 17)
                            {
                                Tingredient tingredient = new Tingredient();
                                tingredient.Ingredient = item.ingredient.ToString();
                                tingredient.IngredientCategoryId = 16;
                                _delicious.Tingredients.Add(tingredient);
                                _delicious.SaveChanges();
                            }
                            TingredientRecord tingredientRecord = new TingredientRecord();
                            if (item.ingredientId == 0)
                            {
                                var _newID = _delicious.Tingredients.Where(n => n.Ingredient == item.ingredient).Select(n => n.IngredientId).FirstOrDefault();
                                item.ingredientId = _newID;
                            }
                            tingredientRecord.IngredientId = Convert.ToInt32(item.ingredientId);
                            tingredientRecord.IngredientAmountForUse = Convert.ToDouble(item.ingredientAmountForUse);
                            tingredientRecord.Unt = item.unt.ToString();
                            tingredientRecord.RecipeId = Convert.ToInt32(ID);
                            _delicious.TingredientRecords.Add(tingredientRecord);
                        }
                        _delicious.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Add_Recipe", r);
                    }
                }

                if (r.list_step != null)
                {
                    if (recipeStep_Check(r.list_step))
                    {
                        int Num = 0;
                        foreach (var item in r.list_step)
                        {
                            Num++;
                            Tstep tstep = new Tstep();
                            if (item.stepphoto != null)
                            {
                                string filename = $"{HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)}_{ID}_step{Num}.jpg";
                                string uploadFolder = Path.Combine(_host.WebRootPath, "assets", "img", "step");
                                string filePath = Path.Combine(uploadFolder, filename);
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    tstep.Picture = @$"/assets/img/step/{filename}";
                                    item.stepphoto.CopyTo(fileStream);
                                }
                            }
                            tstep.RecipeId = ID;
                            tstep.StepsNumber = Num;
                            tstep.Steps = item.step;
                            _delicious.Tsteps.Add(tstep);
                        }
                        _delicious.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Add_Recipe", r);
                    }
                }
                PublishRecipe(recipe.MemberId);

                var follows = _delicious.TmemberFollows.Where(t => t.FollowMemberId == recipe.MemberId).Select(t => t).ToList();
                CEmailSetting mail = new CEmailSetting()
                {
                    MailPort = _CEmailSetting.Value.MailPort,
                    MailServer = _CEmailSetting.Value.MailServer,
                    Password = _CEmailSetting.Value.Password,
                    Sender = _CEmailSetting.Value.Sender,
                    SenderName = _CEmailSetting.Value.SenderName,

                    subject = "您追蹤的使用者發表新食譜囉!"
                };
                mail.MailNewRecipeNotification(ID, recipe.Member.Nickname, follows);

                return RedirectToAction("Recipe", new { id = ID });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "HomePage");
            }
        }

        private bool ingredientRecord_Check(List<CIngredientRecordViewModel> list_ingredientRecords)
        {
            var rule_IngredientAmountForUse = new Regex("^[0-9]+(.[0-9]{1,2})?$");
            var rule_unt = new Regex("^[\u4E00-\u9FA5A-Za-z]+$");

            foreach (var item in list_ingredientRecords)
            {
                if (!rule_IngredientAmountForUse.IsMatch(item.ingredientAmountForUse.ToString()))
                {
                    return false;
                }
                if (!rule_unt.IsMatch(item.unt))
                {
                    return false;
                }
            }
            return true;
        }//後端檢查
        private bool recipeStep_Check(List<CRecipeStepViewModel> list_step)
        {
            foreach (var item in list_step)
            {
                if (item.step == null)
                {
                    return false;
                }
            }
            return true;
        }//後端檢查

        //[Authorize]
        public IActionResult Edit_Recipe(int id) //編輯食譜
        {
            try
            {
                var Recipe = _delicious.Trecipes.Where(a => a.RecipeId == id).Select(a => new
                {
                    recipeId = id,
                    recipePicture = a.Picture,
                    recipeName = a.RecipeName,
                    memberId = a.MemberId,
                    recipeDescription = a.RecipeDescription,
                    recipeCategory = _delicious.TrecipeCategories.Where(j => j.RecipeCategoryId == a.RecipeCategoryId).Select(j => j.RecipeCategory).FirstOrDefault(),
                    recipeCategoryID = a.RecipeCategoryId,
                    recipeVideo = a.Video,
                    _ingredientRecord = _delicious.TingredientRecords.Where(b => b.RecipeId == id).Select(b => new
                    {
                        b.IngredientRecordId,
                        ingredientCategoryId = _delicious.Tingredients.Where(h => h.IngredientId == b.IngredientId).Select(i => i.IngredientCategoryId).FirstOrDefault(),
                        ingredientId = b.IngredientId,
                        ingredient = _delicious.Tingredients.Where(c => c.IngredientId == b.IngredientId).Select(c => c.Ingredient).FirstOrDefault(),
                        ingredientAmountForUse = b.IngredientAmountForUse,
                        unt = b.Unt
                    }).OrderBy(b => b.IngredientRecordId).ToList(),
                    Nickname = _delicious.Tmembers.Where(f => f.MemberId == a.MemberId).Select(f => f.Nickname).FirstOrDefault(),
                    postTime = a.PostTime,
                    forHowmany = a.ForHowMany,
                    timeNeed = a.TimeNeed / 60,
                    views = a.Views,
                    _step = _delicious.Tsteps.Where(g => g.RecipeId == id).Select(g => new
                    {
                        stepPicture = g.Picture,
                        stepNumber = g.StepsNumber,
                        step = g.Steps
                    }).OrderBy(g => g.stepNumber).ToList()
                }).FirstOrDefault();


                CRecipeViewModel cRecipe = new CRecipeViewModel();
                cRecipe.recipeId = id;
                cRecipe.MemberId = Recipe.memberId;
                cRecipe.recipePicture = Recipe.recipePicture;
                cRecipe.recipeName = Recipe.recipeName;
                cRecipe.recipeDescription = Recipe.recipeDescription;
                cRecipe.recipeCategory = Recipe.recipeCategory;
                cRecipe.recipeCategoryId = Recipe.recipeCategoryID;
                cRecipe.recipeVideo = Recipe.recipeVideo;
                cRecipe.Nickname = Recipe.Nickname;
                cRecipe.postTime = Recipe.postTime;
                cRecipe.forHowmany = Recipe.forHowmany;
                cRecipe.timeNeed = Recipe.timeNeed;
                cRecipe.views = Recipe.views;
                foreach (var item in Recipe._ingredientRecord)
                {
                    CIngredientRecordViewModel cIngredientRecord = new CIngredientRecordViewModel();
                    cIngredientRecord.ingredientCategoryId = item.ingredientCategoryId;
                    cIngredientRecord.ingredientId = item.ingredientId;
                    cIngredientRecord.ingredient = item.ingredient;
                    cIngredientRecord.ingredientAmountForUse = item.ingredientAmountForUse;
                    cIngredientRecord.unt = item.unt;
                    cRecipe.list_ingredientRecords.Add(cIngredientRecord);
                }
                foreach (var item in Recipe._step)
                {
                    CRecipeStepViewModel cRecipeStep = new CRecipeStepViewModel();
                    cRecipeStep.stepPicture = item.stepPicture;
                    cRecipeStep.stepNumber = item.stepNumber;
                    cRecipeStep.step = item.step;
                    cRecipe.list_step.Add(cRecipeStep);
                }
                return View(cRecipe);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "HomePage");
            }
        }
        [HttpPost]
        public IActionResult Edit_Recipe(CRecipeViewModel r)
        {
            try
            {
                var Recipe = _delicious.Trecipes.Where(a => a.RecipeId == r.recipeId).Select(n => n).FirstOrDefault();
                Recipe.RecipeName = r.recipeName.ToString();
                Recipe.RecipeCategoryId = Convert.ToInt32(r.recipeCategoryId);
                if (r.recipeVideo != null)
                {
                    Recipe.Video = r.recipeVideo.ToString();
                }
                else
                {
                    Recipe.Video = "";
                }
                if (r.recipephoto != null)
                {
                    string filename = $"{HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)}_{r.recipeName}.jpg";
                    string uploadFolder = Path.Combine(_host.WebRootPath, "assets", "img", "recipe");
                    string filePath = Path.Combine(uploadFolder, filename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Recipe.Picture = @$"/assets/img/recipe/{filename}";
                        r.recipephoto.CopyTo(fileStream);
                    }
                }
                Recipe.RecipeDescription = r.recipeDescription.ToString();
                Recipe.PostTime = DateTime.Now.ToLocalTime();
                Recipe.ForHowMany = Convert.ToInt32(r.forHowmany);
                Recipe.TimeNeed = Convert.ToInt32(r.timeNeed * 60);

                var RecipeIngredientRecord = _delicious.TingredientRecords.Where(b => b.RecipeId == r.recipeId).OrderBy(b => b.IngredientRecordId).Select(c => c).ToList();

                if (r.list_ingredientRecords != null)
                {
                    if (ingredientRecord_Check(r.list_ingredientRecords))
                    {
                        _delicious.TingredientRecords.RemoveRange(RecipeIngredientRecord);
                        foreach (var item in r.list_ingredientRecords)
                        {
                            if (item.ingredientCategoryId == 17)
                            {
                                Tingredient tingredient = new Tingredient();
                                tingredient.Ingredient = item.ingredient.ToString();
                                tingredient.IngredientCategoryId = 16;
                                _delicious.Tingredients.Add(tingredient);
                                _delicious.SaveChanges();
                            }
                            TingredientRecord tingredientRecord = new TingredientRecord();
                            if (item.ingredientId == 0)
                            {
                                var _newID = _delicious.Tingredients.Where(n => n.Ingredient == item.ingredient).Select(n => n.IngredientId).FirstOrDefault();
                                item.ingredientId = _newID;
                            }
                            tingredientRecord.IngredientId = Convert.ToInt32(item.ingredientId);
                            tingredientRecord.IngredientAmountForUse = Convert.ToDouble(item.ingredientAmountForUse);
                            tingredientRecord.Unt = item.unt.ToString();
                            tingredientRecord.RecipeId = Convert.ToInt32(r.recipeId);
                            _delicious.TingredientRecords.Add(tingredientRecord);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Edit_Recipe", new { id = r.recipeId });
                    }
                }
                else
                {
                    _delicious.TingredientRecords.RemoveRange(RecipeIngredientRecord);
                }

                var RecipeStep = _delicious.Tsteps.Where(c => c.RecipeId == r.recipeId).OrderBy(c => c.StepsNumber).Select(d => d).ToList();

                if (r.list_step != null)
                {
                    if (recipeStep_Check(r.list_step))
                    {
                        int Num = 0;
                        for (int i = 0; i < r.list_step.Count; i++)
                        {
                            Num++;
                            Tstep tstep = new Tstep();
                            if (i <= RecipeStep.Count - 1)
                            {
                                if (r.list_step[i].stepphoto != null)
                                {
                                    string filename = $"{r.MemberId}_{r.recipeId}_step{Num}.jpg"; ;
                                    string uploadFolder = Path.Combine(_host.WebRootPath, "assets", "img", "step");
                                    string filePath = Path.Combine(uploadFolder, filename);
                                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                                    {
                                        tstep.Picture = @$"/assets/img/step/{filename}";
                                        r.list_step[i].stepphoto.CopyTo(fileStream);
                                    }
                                }
                                if (RecipeStep[i].Picture != null && r.list_step[i].stepphoto == null)
                                {
                                    tstep.Picture = RecipeStep[i].Picture;
                                }
                            }
                            else
                            {
                                if (r.list_step[i].stepphoto != null)
                                {
                                    string filename = $"{r.MemberId}_{r.recipeId}_step{Num}.jpg"; ;
                                    string uploadFolder = Path.Combine(_host.WebRootPath, "assets", "img", "step");
                                    string filePath = Path.Combine(uploadFolder, filename);
                                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                                    {
                                        tstep.Picture = @$"/assets/img/step/{filename}";
                                        r.list_step[i].stepphoto.CopyTo(fileStream);
                                    }
                                }
                            }
                            tstep.RecipeId = r.recipeId;
                            tstep.StepsNumber = Num;
                            tstep.Steps = r.list_step[i].step;
                            _delicious.Tsteps.Add(tstep);
                        }
                        _delicious.Tsteps.RemoveRange(RecipeStep);
                    }
                    else
                    {
                        return RedirectToAction("Edit_Recipe", new { id = r.recipeId });
                    }
                }
                else
                {
                    _delicious.Tsteps.RemoveRange(RecipeStep);
                }
                _delicious.SaveChanges();

                return RedirectToAction("Recipe", new { id = r.recipeId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "HomePage");
            }
        }//編輯食譜post

        public bool Delete_Recipe(int id)
        {
            try
            {
                var Recipe = _delicious.Trecipes.Where(r => r.RecipeId == id).Select(r => r).FirstOrDefault();
                Recipe.DeleteOrNot = true;
                DeleteRecipe(Recipe.MemberId);
                _delicious.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public IActionResult AddComment(CAddCommentViewModel model)
        {
            if (model.photo != null)
            {
                model.Picture = DateTime.Now.ToLocalTime().ToFileTime().ToString() + model.MemberID.ToString() + ".jpg";
                model.photo.CopyTo(new FileStream(_host.WebRootPath + @"\assets\img\comment\" + model.Picture,
                    FileMode.Create));
            }
            model.PostTime = DateTime.Now.ToLocalTime();
            try
            {
                _delicious.TcommentSections.Add(model._comment);
                if (ScoreCalculation.AddComment(model.MemberID))
                {
                    _delicious.SaveChanges();
                    model._comment.Member = null;
                    return Json(model);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public bool DeleteComment(int id)
        {
            try
            {
                var commentMember = _delicious.TcommentSections.Where(t => t.CommentId == id).Select(t => t.MemberId).FirstOrDefault();
                var deleteComment = _delicious.TcommentSections.Where(t => t.CommentId == id).FirstOrDefault();
                if (ScoreCalculation.DeleteComment(commentMember))
                {
                    deleteComment.DeleteOrNot = true;
                    _delicious.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public bool EditComment(CAddCommentViewModel model)
        {
            var comment = _delicious.TcommentSections.Where(t => t.CommentId == model.CommentID).FirstOrDefault();
            comment.Comment = model.Comment;
            comment.PostTime = DateTime.Now.ToLocalTime();
            if (model.photo != null)
            {
                comment.Picture = DateTime.Now.ToLocalTime().ToFileTime().ToString() + model.MemberID.ToString() + ".jpg";
                model.photo.CopyTo(new FileStream(_host.WebRootPath + @"\assets\img\comment\" + comment.Picture,
                    FileMode.Create));
            }
            else
            {
                comment.Picture = null;
            }
            try
            {
                _delicious.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public bool LikeComment(int commentid, int memberid)
        {
            memberid = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            try
            {
                TlikeComment likeComment = new TlikeComment()
                {
                    CommentId = commentid,
                    MemberId = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID))
                };
                var commentMember = _delicious.TcommentSections.Where(t => t.CommentId == commentid).Select(t => t.MemberId).FirstOrDefault();
                if (ScoreCalculation.LikeComment(commentMember))
                {
                    _delicious.TlikeComments.Add(likeComment);
                    _delicious.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public bool DislikeComment(int commentid, int memberid)
        {
            memberid = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            try
            {
                var dislikeComment = _delicious.TlikeComments.Where(t => t.CommentId == commentid && t.MemberId == Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID))).FirstOrDefault();
                var commentMember = _delicious.TcommentSections.Where(t => t.CommentId == commentid).Select(t => t.MemberId).FirstOrDefault();

                if (dislikeComment != null && ScoreCalculation.DisLikeComment(commentMember))
                {
                    _delicious.TlikeComments.Remove(dislikeComment);
                    _delicious.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        public bool PublishRecipe(int memberId)
        {
            try
            {
                return (new CScoreCalculation(_delicious).GetPoint(memberId));
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteRecipe(int memberId)
        {
            try
            {
                return (new CScoreCalculation(_delicious).LosePoint(memberId));
            }
            catch
            {
                return false;
            }

        }

        public IActionResult LikeRecipeList(int recipeid)
        {
            try
            {
                var like_recipe_member = _delicious.TlikeRecipes.Where(t => t.RecipeId == recipeid).Select(t => new
                {
                    t.MemberId,
                    t.Member.Nickname,
                    figure = _CSrcSetting.Value.FigureSrc + t.Member.Figure,
                });
                return Json(like_recipe_member);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
