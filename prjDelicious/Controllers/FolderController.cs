using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using prjDelicious.Models;
using prjDelicious.Repositories;
using prjDelicious.Services;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class FolderController : Controller
    {
        private readonly IFolderPageService _IFolderPageService;
        private readonly  IFolderPageRepository _IFolderPageRepository;
        public FolderController(DeliciousContext deliciousContext)
        {
            _IFolderPageRepository = new CFolderPageRepository(deliciousContext);
            _IFolderPageService = new CFolderPageService(_IFolderPageRepository);
        }
        //全部收藏頁面
        public IActionResult Index(int memberId)
        {
            if (memberId == 0)
            {
                return RedirectToAction("Login", "HomePage");
            }
            return View(_IFolderPageService.QueryAllMemberFolderAndRecipes(memberId));
        }
        //收藏夾頁面
        public IActionResult MyFolder(int memberId,int folderId)
        {
            if (memberId == 0)
            {
                return RedirectToAction("Login", "HomePage");
            }
            return View(_IFolderPageService.QueryRecipeInFolder(memberId,folderId));
        }
        //食譜收藏夾彈出視窗
        [HttpPost]
        public JsonResult FolderModal(int memberId, int recipeId)
        {
            return Json(_IFolderPageService.QueryFolderWithRecipe(memberId, recipeId));
        }
        //食譜加入收藏夾
        [HttpPost]
        public bool AddFolder(int recipeId , int folderId)
        {
            return _IFolderPageService.InsertRecipeInFolder(recipeId, folderId);
        }
        //食譜移出收藏夾
        [HttpPost]
        public bool RemoveFolder(int recipeId, int folderId)
        {
            return _IFolderPageService.MoveRecipeOutFolder(recipeId, folderId);
        }
        //新增收藏夾
        [HttpPost]
        public bool CreateFolder(string folderName, int memberId)
        {
            return _IFolderPageService.CreateFolder(folderName, memberId);
        }
        //編輯收藏夾名稱
        [HttpPost]
        public bool EditFolderName(string folderName,int folderId)
        {
            return _IFolderPageService.EditFolderName(folderName, folderId);
        }
        //刪除收藏夾
        [HttpPost]
        public bool DeleteFolder(int folderId)
        {
            return _IFolderPageService.DeleteFolder(folderId);
        }
    }
}
