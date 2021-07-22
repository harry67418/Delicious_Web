using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
       
        private readonly IOptions<CEmailSetting> _CEmailSetting;
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public OrderController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext, IOptions<CEmailSetting> CEmailSetting)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
            _CEmailSetting = CEmailSetting;
        }
        public IActionResult Index(string cellnumber,string orderstatus, string email,string orderiD)
        {
            ViewBag.cellnumber = cellnumber;
            ViewBag.orderstatus = orderstatus;
            ViewBag.email = email;
            ViewBag.orderiD = orderiD;
            return View();
        }

        

        public IActionResult EditOrderStatus(int orderID)
        {
            var q = _deliciousContext.TorderDetails.Where(n => n.OrderiD == orderID).Select(n =>
                new
                {
                    OrderId = n.OrderiD,
                    OrderStatus = n.Order.OrderStatus,
                    OrderDate = n.Order.OrderDate,
                    DeliveredDate = n.Order.DeliveredDate,
                    Ingredient = n.Ingredient.Ingredient,
                    InCartQuantity = n.InCartQuantity,
                    Price = n.Price
                });

           

            return Json(q);
        }
        [HttpPost]
        public IActionResult EditOrderStatus(CEditOrderStatusViewModel model)
        {
            if (model.orderStatus.Equals("已取消"))
            {
                var items = _deliciousContext.TorderDetails.Where(n => n.OrderiD == model.orderID).Select(n => new { n.IngredientId, n.InCartQuantity });
                List<TorderDetail> returnquantity = new List<TorderDetail>();

                foreach (var item in items)
                {
                    TorderDetail itemofreturn = new TorderDetail();
                    itemofreturn.IngredientId = item.IngredientId;
                    itemofreturn.InCartQuantity = item.InCartQuantity;
                    returnquantity.Add(itemofreturn);
                }
                foreach (var item in returnquantity)
                { 
                 var quantity = _deliciousContext.Tingredients.FirstOrDefault(n => n.IngredientId == item.IngredientId);
                    quantity.AmountInStore = quantity.AmountInStore + item.InCartQuantity;
                    _deliciousContext.SaveChanges();
                }
                 

            }

            var q = _deliciousContext.Torders.FirstOrDefault(n => n.OrderId == model.orderID);
            q.OrderStatus = model.orderStatus;
            if (model.deliveredDate.Year >= DateTime.Now.Year)
            {
                 q.DeliveredDate = model.deliveredDate; 
            }
            
          
            _deliciousContext.SaveChanges();
            if (q.OrderStatus == "出貨中" || q.OrderStatus == "已取消")
            { SendEmail(model.orderID); }
            return NoContent();
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public IActionResult Create(COrderViewModel Model)
        //{
        //    Torder torder = new Torder();
        //    torder.MemberId = Model.MemberId;
        //    torder.OrderDate = Model.OrderDate;
        //    torder.DeliveryCounty = Model.DeliveryCounty;
        //    torder.DeliveryDistrict = Model.DeliveryDistrict;
        //    torder.DeliveryAddress = Model.DeliveryAddress;
        //    torder.PayMethod = Model.PayMethod;
        //    torder.DeliveredDate = Model.DeliveredDate;
        //    torder.OrderStatus = Model.OrderStatus;
        //    torder.RecieveMethod = Model.RecieveMethod;
        //    _deliciousContext.Torders.Add(torder);
        //    _deliciousContext.SaveChanges();

        //    return View();
        //}


        public IActionResult conditions(CConditonofOrderViewModel Model)
        {
            string orderlist = "";
            CConditonofOrderViewModel NewModel = new CConditonofOrderViewModel();
            if (Model == null)
            {

                orderlist = HttpContext.Session.GetString("conditionsoforder");
                NewModel = JsonConvert.DeserializeObject<CConditonofOrderViewModel>(orderlist);

                Model = NewModel;
            }
            else
            {
                HttpContext.Session.Remove("conditionsoforder");
                HttpContext.Session.SetString("conditionsoforder", JsonConvert.SerializeObject(Model));
            }
          
            //var q = _deliciousContext.Torders.Select(n =>n);
            var q = from p in _deliciousContext.Torders
                    select  new { 
                        OrderId = p.OrderId,
                        MemberId = p.MemberId,
                        OrderDate=p.OrderDate,
                        DeliveredDate=p.DeliveredDate,
                        OrderStatus=p.OrderStatus,
                        RecieveMethod=p.RecieveMethod,
                        PayMethod=p.PayMethod,
                        Member=p.Member.MemberName,
                        CellNumber=p.Member.CellNumber,
                        Email=p.Member.Email


                    };


            q = q.Where(n => n.OrderDate.CompareTo(Model.SelOrderMin.AddHours(-24).AddSeconds(1)) >= 0);
            q = q.Where(n => n.OrderDate.CompareTo(Model.SelOrderMax.AddHours(24).AddSeconds(-1)) <= 0);
            
            if ((Model.SelOrderStatus.Equals("出貨中") || Model.SelOrderStatus.Equals("已送達"))&&(Model.SelDelMin!=Model.SelDelMax))
            {
                q = q.Where(n => n.DeliveredDate.Value >= Model.SelDelMin.AddHours(24).AddSeconds(-1));

                q = q.Where(n => n.DeliveredDate.Value <= Model.SelDelMax.AddHours(24).AddSeconds(-1));
            }
            if (!string.IsNullOrEmpty(Model.SelOrderStatus) &&!Model.SelOrderStatus.Equals("訂單狀態"))
            { q = q.Where(n => n.OrderStatus.Equals(Model.SelOrderStatus)); }
           
            if (Model.SelOrderID != null)
            { q = q.Where(n => n.OrderId.ToString().Equals(Model.SelOrderID)); }
            if (Model.SelMember != null)
            { q = q.Where(n => n.Member.Contains(Model.SelMember)); }
            if (Model.SelPhone != null)
            { q = q.Where(n => n.CellNumber.Equals(Model.SelPhone)); }
            if (Model.SelEmail != null)
            { q = q.Where(n => n.Email.Equals(Model.SelEmail)); }

            if (q == null) { return NoContent(); }

            return Json(q);
          
           
        }

        public bool SendEmail(int orderid)
        {
            var member = _deliciousContext.Torders.Where(m =>m.OrderId==orderid).Select(n=>
            new {
                Email=n.Member.Email,
                MemberName=n.Member.MemberName,
                n.OrderStatus,
                n.PayMethod,
                n.OrderDate,
                }).FirstOrDefault();

            CEmailSetting c = new CEmailSetting()
            {
                MailPort = _CEmailSetting.Value.MailPort,
                MailServer = _CEmailSetting.Value.MailServer,
                Password = _CEmailSetting.Value.Password,
                Sender = _CEmailSetting.Value.Sender,
                SenderName = _CEmailSetting.Value.SenderName,
                subject = "瘋廚網_訂單狀態通知"
            };
            return c.MailOrderStatus(member.Email, member.MemberName, member.OrderStatus,member.PayMethod,member.OrderDate.ToShortDateString());
        }
    }
}
