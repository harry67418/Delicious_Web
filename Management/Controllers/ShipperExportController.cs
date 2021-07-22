using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Authorize]
    public class ShipperExportController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ShipperExportController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
      

   
        public async Task<IActionResult> GroupExport(CConditonofOrderViewModel Model)
        {
            await Task.Yield();
            //CConditonofOrderViewModel Model = new CConditonofOrderViewModel();

            var q = _deliciousContext.Torders.Select(n => n);
            q = q.Where(n => n.OrderDate.CompareTo(Model.SelOrderMin.AddHours(-24).AddSeconds(1)) >= 0);
            q = q.Where(n => n.OrderDate.CompareTo(Model.SelOrderMax.AddHours(24).AddSeconds(-1)) <= 0);

            if (!string.IsNullOrEmpty(Model.SelOrderStatus) && !Model.SelOrderStatus.Equals("訂單狀態"))
            { q = q.Where(n => n.OrderStatus.Equals(Model.SelOrderStatus)); }

            if (Model.SelOrderID != null)
            { q = q.Where(n => n.OrderId.ToString().Equals(Model.SelOrderID)); }
            if (Model.SelMember != null)
            { q = q.Where(n => n.Member.MemberName.Contains(Model.SelMember)); }
            if (Model.SelPhone != null)
            { q = q.Where(n =>n.Member.CellNumber.Equals(Model.SelPhone)); }
            if (Model.SelEmail != null)
            { q = q.Where(n => n.Member.Email.Equals(Model.SelEmail)); }
           
            if ((Model.SelOrderStatus.Equals("出貨中") || Model.SelOrderStatus.Equals("已送達")) && (Model.SelDelMin != Model.SelDelMax))
            {
                q = q.Where(n => n.DeliveredDate.Value >= Model.SelDelMin.AddSeconds(-1));

                q = q.Where(n => n.DeliveredDate.Value <= Model.SelDelMax.AddHours(24).AddSeconds(-1));
            }


            if (q.Count() > 0) { 

            List<COrderViewModel> orders = new List<COrderViewModel>();
            foreach (var item in q)
            {
                COrderViewModel one = new COrderViewModel();
                one.OrderId = item.OrderId;
                one.OrderStatus = item.OrderStatus;
                one.PayMethod = item.PayMethod;
                one.OrderDate = item.OrderDate.ToString("yyyy/MM/dd");
                one.DeliveryCounty = item.DeliveryCounty;
                if (item.DeliveredDate != null)
                {
                    DateTime newdate = new DateTime();
                    newdate = item.DeliveredDate.Value;
                    one.DeliveredDate = newdate.ToString("yyyy/MM/dd");
                }
                orders.Add(one);
              
            }
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);

            foreach (var order in orders)
            {

                var p = _deliciousContext.TorderDetails.Where(n => n.OrderiD == order.OrderId).Select(n =>
           new
           {
               merchadise = n.Ingredient.Ingredient,
               InCartQuantity = n.InCartQuantity,
               n.Price,
               totalprice = n.InCartQuantity * n.Price,
               n.OrderiD,
               OrderStatus = n.Order.OrderStatus,
               Reciever = n.Order.Reciever,
               PayMethod = n.Order.PayMethod,
               recieveMethod = n.Order.RecieveMethod,
               Orderdate = n.Order.OrderDate,
               CellNumber = n.Order.PhoneNumber,
               Address = n.Order.DeliveryCounty + n.Order.DeliveryDistrict + n.Order.DeliveryAddress


           });
                var shiptitle = new CShippermentTitleViewModel();

                shiptitle.cellNumber = p.FirstOrDefault().CellNumber;
                shiptitle.memberName = p.FirstOrDefault().Reciever;
                shiptitle.payMethod = p.FirstOrDefault().PayMethod;
                shiptitle.recieveMethod = p.FirstOrDefault().recieveMethod;
                shiptitle.orderDate = p.FirstOrDefault().Orderdate.ToString("yyyy/MM/dd");
                shiptitle.address = p.FirstOrDefault().Address;
                shiptitle.orderstatus = p.FirstOrDefault().OrderStatus;

                List<CShippermentViewModel> shipreport = new List<CShippermentViewModel>();
                foreach (var item in p)
                {
                    CShippermentViewModel aitem = new CShippermentViewModel();
                    aitem.merchadise = item.merchadise;
                    aitem.quantity = item.InCartQuantity;
                    aitem.unitprice = item.Price;

                    shipreport.Add(aitem);
                }

                foreach (var i in shipreport)
                { shiptitle.totalprice += i.unitprice * i.quantity; }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;



                var ws = package.Workbook.Worksheets.Add(Name: "訂單編號" + order.OrderId);
                var range = ws.Cells[Address: "B11"].LoadFromCollection(shipreport, PrintHeaders: true);
                ws.Cells[Address: "A1"].Value = "瘋廚網有限公司" + "電話:02 6631 6599";
                ws.Cells[Address: "A2"].Value = "寄件地址:106 臺北市大安區復興南路一段390號2樓";

                ws.Cells[Address: "A4"].Value = "收件人:" + shiptitle.memberName + "  手機號碼:" + shiptitle.cellNumber;
                ws.Cells[Address: "A5"].Value = "配送地址" + shiptitle.address;
                ws.Cells[Address: "A6"].Value = "會員姓名:" + shiptitle.memberName;



                ws.Cells[Address: "A8"].Value = shiptitle.orderstatus;
                ws.Cells[Address: "B8"].Value = "------------------------------------------------------------";
                ws.Cells[Address: "A9"].Value = "瘋廚網銷貨單";
                ws.Cells[Address: "A10"].Value = "訂單日期" + shiptitle.orderDate + "會員姓名" + shiptitle.memberName;
                ws.Cells[Address: "A11"].Value = "序號";
                ws.Cells[Address: "A1:F1"].Merge = true;
                ws.Cells[Address: "A2:F2"].Merge = true;
                ws.Cells[Address: "A3:F3"].Merge = true;
                ws.Cells[Address: "A4:F4"].Merge = true;
                ws.Cells[Address: "A5:F5"].Merge = true;
                ws.Cells[Address: "A6:F6"].Merge = true;
                ws.Cells[Address: "A7:F7"].Merge = true;
                ws.Cells[Address: "B8:F8"].Merge = true;
                ws.Cells[Address: "A9:F9"].Merge = true;
                ws.Cells[Address: "A10:F10"].Merge = true;

                string totalprice = "D" + (shipreport.Count() + 12);
                string words = "C" + (shipreport.Count() + 12);
                string PayMethodstart = "A" + (shipreport.Count() + 13);
                ws.Cells[Address: PayMethodstart].Value = "付款方式:" + shiptitle.payMethod;
                string PayMethodend = PayMethodstart + ":C" + (shipreport.Count() + 13);
                ws.Cells[Address: PayMethodend].Merge = true;

                ws.Cells[Address: totalprice].Value = "$" + shiptitle.totalprice;
                ws.Cells[Address: words].Value = "總金額";
                int j = 12;
                for (int i = 1; i <= shipreport.Count(); i++)
                {

                    string countword = "A" + j;
                    ws.Cells[Address: countword].Value = i.ToString();
                    j++;
                }
                for (int i = 1; i <= 100; i++)
                {
                    ws.Row(row: i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                }
                range.AutoFitColumns();
            }
                   
                    package.Save();
               
                stream.Position = 0;
                 var filename = DateTime.Now.ToString("yyyyMMddhhmm") + "訂單明細.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);

            }
            return null;


        }
        public async Task<IActionResult> SingleExport(int orderid)
        {
            await Task.Yield();
            var p = _deliciousContext.TorderDetails.Where(n => n.OrderiD == orderid).Select(n =>
            new
            {
                merchadise = n.Ingredient.Ingredient,
                InCartQuantity = n.InCartQuantity,
                n.Price,
                totalprice = n.InCartQuantity * n.Price,
                n.OrderiD,
                OrderStatus=n.Order.OrderStatus,
                Reciever = n.Order.Reciever,
                PayMethod = n.Order.PayMethod,
                recieveMethod = n.Order.RecieveMethod,
                Orderdate = n.Order.OrderDate,
                CellNumber=n.Order.PhoneNumber,
                Address=n.Order.DeliveryCounty+n.Order.DeliveryDistrict+n.Order.DeliveryAddress


            });
            var shiptitle = new CShippermentTitleViewModel();

            shiptitle.cellNumber = p.FirstOrDefault().CellNumber;
            shiptitle.memberName = p.FirstOrDefault().Reciever;
            shiptitle.payMethod = p.FirstOrDefault().PayMethod;
            shiptitle.recieveMethod = p.FirstOrDefault().recieveMethod;
            shiptitle.orderDate = p.FirstOrDefault().Orderdate.ToString("yyyy/MM/dd");
            shiptitle.address = p.FirstOrDefault().Address;
            shiptitle.orderstatus = p.FirstOrDefault().OrderStatus;
            
            List<CShippermentViewModel> shipreport = new List<CShippermentViewModel>();
            foreach (var item in p)
            {
                CShippermentViewModel aitem = new CShippermentViewModel();
                aitem.merchadise = item.merchadise;
                aitem.quantity = item.InCartQuantity;
                aitem.unitprice = item.Price;

                shipreport.Add(aitem);
            }
           
            foreach (var i in shipreport)
            { shiptitle.totalprice += i.unitprice * i.quantity; }
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {

                var ws = package.Workbook.Worksheets.Add(Name: "MainReport");
                var range = ws.Cells[Address: "B11"].LoadFromCollection(shipreport, PrintHeaders: true);
                ws.Cells[Address: "A1"].Value = "瘋廚網有限公司" + "電話:02 6631 6599";
                ws.Cells[Address: "A2"].Value = "寄件地址:106 臺北市大安區復興南路一段390號2樓";

                ws.Cells[Address: "A4"].Value = "收件人:" + shiptitle.memberName + "  手機號碼:" + shiptitle.cellNumber;
                ws.Cells[Address: "A5"].Value = "配送地址" + shiptitle.address;
                ws.Cells[Address: "A6"].Value = "會員姓名:" + shiptitle.memberName;



                ws.Cells[Address: "A8"].Value = shiptitle.orderstatus;
                ws.Cells[Address: "B8"].Value = "------------------------------------------------------------";
                ws.Cells[Address: "A9"].Value = "瘋廚網銷貨單";
                ws.Cells[Address: "A10"].Value = "訂單日期" + shiptitle.orderDate + "會員姓名" + shiptitle.memberName;
                ws.Cells[Address: "A11"].Value = "序號";
                ws.Cells[Address: "A1:F1"].Merge = true;
                ws.Cells[Address: "A2:F2"].Merge = true;
                ws.Cells[Address: "A3:F3"].Merge = true;
                ws.Cells[Address: "A4:F4"].Merge = true;
                ws.Cells[Address: "A5:F5"].Merge = true;
                ws.Cells[Address: "A6:F6"].Merge = true;
                ws.Cells[Address: "A7:F7"].Merge = true;
                ws.Cells[Address: "B8:F8"].Merge = true;
                ws.Cells[Address: "A9:F9"].Merge = true;
                ws.Cells[Address: "A10:F10"].Merge = true;

                string totalprice = "D" + (shipreport.Count() + 12);
                string words = "C" + (shipreport.Count() + 12);
                string PayMethodstart = "A" + (shipreport.Count() + 13);
                ws.Cells[Address: PayMethodstart].Value = "付款方式:" + shiptitle.payMethod;
                string PayMethodend = PayMethodstart + ":C" + (shipreport.Count() + 13);
                ws.Cells[Address: PayMethodend].Merge = true;

                ws.Cells[Address: totalprice].Value = "$" + shiptitle.totalprice;
                ws.Cells[Address: words].Value = "總金額";
                int j = 12;
                for (int i = 1; i <= shipreport.Count(); i++)
                {

                    string countword = "A" + j;
                    ws.Cells[Address: countword].Value = i.ToString();
                    j++;
                }
                for (int i = 1; i <= 100; i++)
                {
                    ws.Row(row: i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                }

                range.AutoFitColumns();
                package.Save();
            }
            stream.Position = 0;
          

            var filename = shiptitle.orderDate.Replace("/", "") + "出貨單" + orderid+".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);


           

        }

    }
}
