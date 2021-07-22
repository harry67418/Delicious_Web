using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebChart.ViewModels;

namespace WebChart.Controllers
{
    
    public class ChatApiController : ControllerBase
    {
        /// <summary>
        /// 線上客服
        /// </summary>
        private readonly IWebHostEnvironment _hostingEnvironment;
         
        public ChatApiController(  IWebHostEnvironment hostingEnvironment)
        {
             
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult ImageInput(CImageInput _cImage)
        {
            //將檔案存到uploads資料夾中
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "chatimg"); //c:\......\uploads
            string FileName = Guid.NewGuid().ToString() + ".jpg";
            string filePath = Path.Combine(uploadsFolder, FileName );//c:\......\uploads\cats.jpg            
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
            return File("~/chatimg/" + fileName, "image/jpeg");
        }
    }
}
