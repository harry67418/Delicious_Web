using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        public OrderDetailController(DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
        }

        public IActionResult List(int memberid,string status)
        {
            ViewBag.status = status;
            if (status=="全部")
            {
                status = null;
            }
            List<COrderViewModel> modelList = getdatas(memberid,status);
            return View(modelList);
        }

        private List<COrderViewModel> getdatas(int memberid,string status)
        {
            List<COrderViewModel> modelList = new List<COrderViewModel>();//最後回傳的是多筆訂單
            var orders = _deliciousContext.Torders.Where(m => m.MemberId == memberid).Select(o => o).OrderByDescending(o=>o.OrderId).ToList();//查詢使用者所有的訂單
            if(status !=null)
            {
                orders = orders.Where(n => n.OrderStatus == status).ToList();
            }

            if (orders != null)
            {
                foreach (var item in orders)//迴圈把單筆訂單抓出來找訂單細項
                {
                    var orderDetails = _deliciousContext.TorderDetails.Where(o => o.OrderiD == item.OrderId).Select(n => new
                    {
                        n,
                        n.Ingredient.Ingredient,
                        n.Ingredient.IngredientUnit,
                        小計 = n.InCartQuantity * n.Price
                    }).ToList();//篩選出該筆訂單的所有細項,還有食材名稱

                    decimal totalprice = 0;//用來算總計

                    List<COrderDetailViewModel> detailViewModels = new List<COrderDetailViewModel>();//建立訂單細項的容器
                    foreach (var detail in orderDetails)//迴圈把單筆細項抓出來放進viewmodel
                    {
                        COrderDetailViewModel details = new COrderDetailViewModel();//建立單筆細項的viewmodel
                        details._orderdetail = detail.n;//單筆細項的資訊
                        details.Ingredient = detail.Ingredient;//單筆細項的食材名稱
                        details.Unit = detail.IngredientUnit;
                        totalprice += detail.小計;
                        detailViewModels.Add(details);//加入細項的容器
                    }//這邊處理完之後多筆細項就完成了

                    COrderViewModel model = new COrderViewModel();//單筆訂單的資訊
                    model._order = item;
                    model.totalPrice = totalprice;
                    model.orderdetailList.AddRange(detailViewModels);
                    modelList.Add(model);//單筆訂單加入多筆訂單的容器

                    totalprice = 0;
                }
            }
            return modelList;
        }

        public string Edit(int orderid)
        {
            var editStatus = _deliciousContext.Torders.FirstOrDefault(o => o.OrderId == orderid);
            editStatus.OrderStatus = "已取消";
            _deliciousContext.SaveChanges();

            //取消的訂單回填庫存量
            var INGs = _deliciousContext.TorderDetails.Where(I => I.OrderiD == orderid).Select(I => new
            {
                I.IngredientId,
                I.InCartQuantity
            });
            List<TorderDetail> list = new List<TorderDetail>();
            foreach(var item in INGs)
            {
                TorderDetail torderDetail = new TorderDetail();
                torderDetail.IngredientId = item.IngredientId;
                torderDetail.InCartQuantity = item.InCartQuantity;
                list.Add(torderDetail);
            }
            foreach(var item in list)
            {
                var ING = _deliciousContext.Tingredients.FirstOrDefault(n => n.IngredientId == item.IngredientId);
                ING.AmountInStore += item.InCartQuantity;
                _deliciousContext.SaveChanges();
            }            
            return "取消此筆訂單";           
        }        
    }
}
