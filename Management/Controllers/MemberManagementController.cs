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
    public class MemberManagementController : Controller
    {

        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public MemberManagementController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
          
            return View();
        }
        public IActionResult Getmembers(string keywords) {
           
            if (string.IsNullOrEmpty(keywords))
            {
                var members = _deliciousContext.Tmembers.Select(n =>
              new
              {
                  n.MemberId,
                  n.MemberName,
                  n.AccountName,
                  n.Birthday,
                  n.Email,
                  n.CellNumber,
              });
                CMemberInfosViewModel memberInfos = new CMemberInfosViewModel();
                foreach (var member in members)
                {
                    CMemberInfoItemsViewModel amember = new CMemberInfoItemsViewModel();
                    amember.MemberId = member.MemberId;
                    amember.MemberName = member.MemberName;
                    amember.AccountName = member.AccountName;

                    if (member.Birthday.ToString() != "")
                    { amember.Birthday = member.Birthday.Value.Year.ToString()+"/"+ member.Birthday.Value.Month.ToString() + "/"+ member.Birthday.Value.Day.ToString(); }

                    amember.Email = member.Email;

                    if (member.CellNumber != null)
                    { amember.CellNumber = member.CellNumber; } 
                    
                    memberInfos.memberlist.Add(amember);
                }
                return Json(memberInfos);
            }
            else 
            {
               
                var members = _deliciousContext.Tmembers.Where(n => n.MemberId.ToString().Contains(keywords) || n.MemberName.Contains(keywords) || n.AccountName.Contains(keywords) ||  n.Birthday.ToString().Contains(keywords) ||  n.Email.Contains(keywords) || n.CellNumber.Contains(keywords)).Select(n =>
            new
            {
                n.MemberId,
                n.MemberName,
                n.AccountName,
                n.Birthday,
                n.Email,
                n.CellNumber,
            });
                CMemberInfosViewModel memberInfos = new CMemberInfosViewModel();
                foreach (var member in members)
                {
                    CMemberInfoItemsViewModel amember = new CMemberInfoItemsViewModel();
                    amember.MemberId = member.MemberId;
                    amember.MemberName = member.MemberName;
                    amember.AccountName = member.AccountName;
                    amember.Birthday = member.Birthday.ToString();
                    amember.Email = member.Email;
                    amember.CellNumber = member.CellNumber;
                    memberInfos.memberlist.Add(amember);
                }
                return Json(memberInfos);

            }
           
           
        }

        public IActionResult ActivityRecords(int memberid)
        {
            CMemberInfoItemsViewModel amember = new CMemberInfoItemsViewModel();

            var memberinfo= _deliciousContext.Tmembers.Single(n => n.MemberId == memberid);
            amember.MemberId = memberid;
            amember.MemberName = memberinfo.MemberName;
            amember.Email = memberinfo.Email;
            amember.Nickname = memberinfo.Nickname;
            amember.CellNumber = memberinfo.CellNumber;
            amember.PersonalRankScore = memberinfo.PersonalRankScore;
            var recipes =_deliciousContext.Trecipes.Where(n => n.MemberId == memberid).OrderBy(n=>n.PostTime).Select(n=>n);
            amember.recipecount = recipes.Count();
            foreach (var item in recipes)
            {
                amember.trecipelist.Add( item.PostTime.ToString("yyyy/MM/dd")+"\n"  + "食譜編號\n" + item.RecipeId + "\n\n"+ item.RecipeName);
            }
            var orders=_deliciousContext.Torders.Where(n => n.MemberId == memberid).OrderBy(n=>n.OrderDate).Select(n => n);
            amember.ordercount = orders.Count();
            foreach (var item in orders)
            {
                amember.ordertime.Add(item.OrderDate.ToString("yyyy/MM/dd")+ "\n" + "訂單編號\n"+item.OrderId+ "\n\n" );
            }
            var contribute = _deliciousContext.TphotoWalls.Where(n => n.MemberId == memberid).OrderBy(n => n.ContributeTime).Select(n => new
            {
                CategoryName = n.Category.CategoryName,
                ContributeTime=n.ContributeTime

            }) ;
            
            amember.contributephotowallcount = contribute.Count();
            foreach(var item in contribute)
            {
                amember.contributephotowalltime.Add(item.ContributeTime.ToString("yyyy/MM/dd")+ "\n投稿分類\n" + item.CategoryName);

            }


            return  Json(amember);
           
        }


    }
}
