using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class OPayController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        public OPayController(DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        //接收回傳
        [HttpPost]
        public IActionResult Postback(COpayPostback model,string CheckMacValue)
        {
            try
            {
                var torder = _deliciousContext.Torders.Where(t => t.MerchantTradeNo == model.MerchantTradeNo).FirstOrDefault();
                if (torder != null && model.RtnCode ==1 && CheckMacValue == model.MyCheckMacValue)
                {
                    torder.OrderStatus = "已付款";
                    _deliciousContext.SaveChanges();
                    return Json("成功");
                }
                else
                {
                    return Json("失敗");   
                }
                
            }
            catch (Exception ex)
            {
                return Json("失敗");
            }
        }
        //送出付款
        public IActionResult SetParameter(int orderid)
        {
            var orders = _deliciousContext.TorderDetails.Where(t => t.Order.OrderId == orderid).Select(t=>t);

            if (orders != null)
            {
                string itemName = "";
                decimal TotalAmount = 0;
                foreach (var order in orders)
                {
                    string item = $"{order.Ingredient.Ingredient}  ({order.Price}/{order.Ingredient.IngredientUnit})x{order.InCartQuantity}#";
                    itemName += item;
                    TotalAmount += order.Price * order.InCartQuantity;
                }
                itemName.Remove(itemName.Length - 1, 1);

                COpayCreateOrder cOpay = new COpayCreateOrder();
                cOpay.MerchantID = "2000132";
                cOpay.MerchantTradeNo = "DE" + DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmssff");
                cOpay.MerchantTradeDate = DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                cOpay.PaymentType = "aio";//交易類型(規定填入aio)
                cOpay.TotalAmount = Convert.ToInt32(TotalAmount);//***交易總金額
                cOpay.TradeDesc = "建立信用卡測試訂單";//***交易描述
                cOpay.ItemName = itemName;//***商品名稱,多筆用#換行

                //付款完成通知回傳網址(用來呼叫ACTION接收回傳參數,更改資料庫付款狀態)
                cOpay.ReturnURL = "https://deliciousnet.azurewebsites.net/Opay/Postback";
                cOpay.ChoosePayment = "Credit";//預設付款方式(規定填入Credit)
                cOpay.ClientBackURL = "https://deliciousnet.azurewebsites.net/OrderDetail/List?memberid=" + HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID) + "&status=已付款";
                cOpay.EncryptType = 1;//CheckMacValue檢查碼在方法內產生並為唯讀

                orders.FirstOrDefault().Order.MerchantTradeNo = cOpay.MerchantTradeNo;//更新該筆資料的編碼


                try
                {
                    _deliciousContext.SaveChanges();
                    return View(cOpay);
                }
                catch (Exception ex)
                {
                    return Content("發生錯誤");
                }
            }
            else
            {
                return Content("發生錯誤");
            }
        }

        //public IActionResult PostToOpay(int orderid)
        //{
        //    var orders = _deliciousContext.TorderDetails.Where(t => t.Order.OrderId == orderid).Select(t => t);

        //    if (orders != null)
        //    {
        //        string itemName = "";
        //        decimal TotalAmount = 0;
        //        foreach (var order in orders)
        //        {
        //            string item = $"{order.Ingredient.Ingredient}  {order.Price}x{order.InCartQuantity}{order.Ingredient.IngredientUnit}#";
        //            itemName += item;
        //            TotalAmount += order.Price * order.InCartQuantity;
        //        }
        //        itemName.Remove(itemName.Length - 1, 1);

        //        COpayCreateOrder cOpay = new COpayCreateOrder();
        //        cOpay.MerchantID = "2000132";
        //        cOpay.MerchantTradeNo = "DE" + DateTime.Now.ToString("yyyyMMddHHmmssff");
        //        cOpay.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //        cOpay.PaymentType = "aio";//交易類型(規定填入aio)
        //        cOpay.TotalAmount = Convert.ToInt32(TotalAmount);//***交易總金額
        //        cOpay.TradeDesc = "建立信用卡測試訂單";//***交易描述
        //        cOpay.ItemName = itemName;//***商品名稱,多筆用#換行

        //        //付款完成通知回傳網址(用來呼叫ACTION接收回傳參數,更改資料庫付款狀態)
        //        cOpay.ReturnURL = "https://deliciousnet.azurewebsites.net/Opay/Postback";
        //        cOpay.ChoosePayment = "Credit";//預設付款方式(規定填入Credit)
        //        cOpay.ClientBackURL = "https://deliciousnet.azurewebsites.net/OrderDetail/List?memberid=" + HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID) + "&status=已付款";
        //        cOpay.EncryptType = 1;//CheckMacValue檢查碼在方法內產生並為唯讀

        //        orders.FirstOrDefault().Order.MerchantTradeNo = cOpay.MerchantTradeNo;//更新該筆資料的編碼
        //        try
        //        {
        //            _deliciousContext.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            return Content("發生錯誤");
        //        }
        //        string jsonString = JsonConvert.SerializeObject(cOpay);
        //        var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
        //        HttpClient _httpClient = new HttpClient();
        //        using (content)
        //        {
        //            HttpResponseMessage result = _httpClient.PostAsync("https://payment-stage.opay.tw/Cashier/AioCheckOut/V5", content).Result;
        //            return Json(result);
        //        }
        //    }
        //    else
        //    {
        //        return Content("發生錯誤");
        //    }
        //}
    }
}
