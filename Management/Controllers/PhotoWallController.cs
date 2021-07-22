using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Authorize]
    public class PhotoWallController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PhotoWallController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
 
            CPhotoWallViewModel cPhotoWallViewModel = new CPhotoWallViewModel();

           var cates = _deliciousContext.TphotoWallCategories.Select(n => n).Distinct();
            foreach (var item in cates)
            {
                CPhotoWallItemViewModel newitem = new CPhotoWallItemViewModel();
                newitem.Category = item.CategoryName;
                newitem.CategoryId = item.CategoryId;
                cPhotoWallViewModel.list.Add(newitem);
            }
            foreach (var item in cPhotoWallViewModel.list)
            {
                item.CounttheSame = _deliciousContext.TphotoWalls.Where(n => n.CategoryId == item.CategoryId && n.Display).Select(n => n).Count();
            }

                return View(cPhotoWallViewModel);

        }



        public IActionResult listbycategory()
        {
            var q = _deliciousContext.TphotoWalls.OrderBy(n=>n.CategoryId).Select(n => new { n.CategoryId,Category=n.Category.CategoryName }).Distinct();
            CPhotoWallViewModel cPhotoWallViewModel = new CPhotoWallViewModel();
            foreach (var item in q)
            {
                CPhotoWallItemViewModel photowallitem = new CPhotoWallItemViewModel();
                photowallitem.Category = item.Category;
                photowallitem.CategoryId = item.CategoryId;
               
                cPhotoWallViewModel.list.Add(photowallitem);
            }
            cPhotoWallViewModel.categorycount = _deliciousContext.TphotoWallCategories.Count();
            foreach (var item in cPhotoWallViewModel.list)
            {
                item.CounttheSame = _deliciousContext.TphotoWalls.Where(n => n.CategoryId == item.CategoryId && n.Display).Select(n=>n).Count();
            }

            return View(cPhotoWallViewModel);
        }
         
        public IActionResult list(string keywords)
        {
            var q = _deliciousContext.TphotoWalls.OrderByDescending(n => n.ContributeTime).Select(n =>
           new
           {
               PictureId = n.PictureId,
               AccountName = n.Member.AccountName,
               Categoery = n.Category.CategoryName,
               CategoeryID = n.CategoryId,
               Picture = n.Picture,
               Display = n.Display,
               ContributeTime = n.ContributeTime

           });
            if (keywords != "全部種類")
            {
                q = q.Where(n => n.Categoery == keywords);
            
            }
            CPhotoWallViewModel cPhotoWallViewModel = new CPhotoWallViewModel();

            foreach (var item in q)
            {
                CPhotoWallItemViewModel itemViewModel = new CPhotoWallItemViewModel();
                itemViewModel.PictureId = item.PictureId;
                itemViewModel.MemberAccountName = item.AccountName;
                itemViewModel.Category = item.Categoery;
                itemViewModel.Picture = item.Picture;
                itemViewModel.Display = item.Display;
                itemViewModel.ContributeTime = item.ContributeTime.ToString("yyyy/MM/dd");
                itemViewModel.CategoryId = item.CategoeryID;
                cPhotoWallViewModel.list.Add(itemViewModel);

            }
            foreach (var item in cPhotoWallViewModel.list)
            {
                item.CounttheSame = _deliciousContext.TphotoWalls.Where(n => n.CategoryId == item.CategoryId && n.Display).Select(n => n).Count();
            }


            return Json(cPhotoWallViewModel);

        }

        
        public IActionResult savestatus(int picid)
        {
            var q = _deliciousContext.TphotoWalls.FirstOrDefault(n => n.PictureId == picid);
            if (q.Display) { q.Display = false; }
            else { q.Display = true; }
            _deliciousContext.SaveChanges();
            return NoContent();
        }


        }
    }
