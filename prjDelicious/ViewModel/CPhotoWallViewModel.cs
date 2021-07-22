using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CPhotoWallViewModel
    {
        public TphotoWall _photowall { get; set; }
        public IEnumerable<TphotoWallCategory> _categorys { get; set; }
        public CPhotoWallViewModel()
        {
            _photowall = new TphotoWall();
        }
        public string CategoryName { get; set; }
        public string HtmlClassName { get; set; }
        public string NickName { get; set; }
        public int MemberId
        {
            get { return _photowall.MemberId; }
            set { _photowall.MemberId = value; }
        }
        [Required(ErrorMessage = "請選擇分類")]
        public int CategoryID
        {
            get { return _photowall.CategoryId; }
            set { _photowall.CategoryId = value; }
        }

        public string Picture 
        {
            get { return _photowall.Picture; }
            set { _photowall.Picture = value; } 
        }

        public IFormFile photo { get; set; }

        public bool InsertPhotoWall(CPhotoWallViewModel model, DeliciousContext _deliciousContext, IWebHostEnvironment _hostingEnvironment)
        {
            model._photowall.ContributeTime = DateTime.Now.ToLocalTime();
            model._photowall.Display = false;
            if (model.photo != null)//如果圖片有更改
            {
                model.Picture = DateTime.Now.ToLocalTime().ToFileTime().ToString() + model.MemberId.ToString() + ".jpg";
                model.photo.CopyTo(new FileStream(_hostingEnvironment.WebRootPath+@"\assets\img\wall\" + model.Picture,
                    FileMode.Create));
            }
            try
            {
                _deliciousContext.TphotoWalls.Add(model._photowall);
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
