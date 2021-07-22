using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Authorize]
    public class ShowingPicsController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ShowingPicsController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var p = _deliciousContext.TshowingPics.Select(n => n);
            return View(p);
        }

        public int CountTrue()
        {
            var p = _deliciousContext.TshowingPics.Where(n => n.ShowingPicsIsShowOrNot == true).Select(n => n).Count();

            return p;
        }

        public IActionResult Edit(int ShowingPicsId)
        {
            int Showingpicid = ShowingPicsId;
            var p = _deliciousContext.TshowingPics.FirstOrDefault(n => n.ShowingPicsId == Showingpicid);
            CShowingPicViewModel pic = new CShowingPicViewModel();
            pic.ShowingPicsId = ShowingPicsId;
            pic.ShowingPicsTitle = p.ShowingPicsTitle;
            pic.ShowingPicsDescription = p.ShowingPicsDescription;
            pic.ShowingPicsHyperLink = p.ShowingPicsHyperLink;
            pic.ShowingPicsPathRoad = p.ShowingPicsPathRoad;
            pic.ShowingPicsIsShowOrNot = p.ShowingPicsIsShowOrNot;
            pic.counttrue = CountTrue();
            return View(pic);
        }
        [HttpPost]
        public IActionResult Edit(CShowingPicViewModel model)
        {
            int Showingpicid = model.ShowingPicsId;
            var pic = _deliciousContext.TshowingPics.FirstOrDefault(n => n.ShowingPicsId == Showingpicid);


            pic.ShowingPicsTitle = model.ShowingPicsTitle;
            pic.ShowingPicsDescription = model.ShowingPicsDescription;
            pic.ShowingPicsHyperLink = model.ShowingPicsHyperLink;

            pic.ShowingPicsIsShowOrNot = model.ShowingPicsIsShowOrNot;
            pic.ShowingPicsPathRoad = model.ShowingPicsPathRoad;

            if (model.uploadpic != null)
            {
                string filename = (DateTime.Now).ToString("yyyyMMddhhmmss") + pic.ShowingPicsId + ".jpg";
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img", "ShowingPic");
                string filePath = Path.Combine(uploadFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.uploadpic.CopyTo(fileStream);
                }
                pic.ShowingPicsPathRoad = filename;

            }
            _deliciousContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            CShowingPicViewModel pic = new CShowingPicViewModel();
            pic.counttrue = CountTrue();
            return View(pic);
        }
        [HttpPost]
        public IActionResult Create(CShowingPicViewModel tshowing)
        {
            TshowingPic pic = new TshowingPic();

            pic.ShowingPicsId = tshowing.ShowingPicsId;
            pic.ShowingPicsDescription = tshowing.ShowingPicsDescription;
            pic.ShowingPicsHyperLink = tshowing.ShowingPicsHyperLink;
            pic.ShowingPicsTitle = tshowing.ShowingPicsTitle;
            pic.ShowingPicsIsShowOrNot = tshowing.ShowingPicsIsShowOrNot;


            if (tshowing.uploadpic != null)
            {
                string filename = (DateTime.Now).ToString("yyyyMMddhhmmss") + pic.ShowingPicsId + ".jpg";
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img", "ShowingPic");
                string filePath = Path.Combine(uploadFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    tshowing.uploadpic.CopyTo(fileStream);
                }
                pic.ShowingPicsPathRoad = filename;

            }
            _deliciousContext.TshowingPics.Add(pic);
            _deliciousContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int ShowingPicsId)
        {
            var pic = _deliciousContext.TshowingPics.FirstOrDefault(n => n.ShowingPicsId == ShowingPicsId);
            _deliciousContext.Remove(pic);
            _deliciousContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
