using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.Common;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.IdStrategy;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Ace.Web.Mvc.Models;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Mvc;

namespace Chloe.Admin.Areas.Wiki.Controllers
{

    [Area(AreaNames.Wiki)]
    [Permission("wiki.news")]
    public class NewsController : WebController<INewsService>
    {
        public IActionResult Index()
        {
            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList( (int)EnumSort.News, "");
            ViewBag.ListSort = ListSort;

            return View();
        }

        [Permission("wiki.comment")]
        public IActionResult Comment()
        {
            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList((int)EnumSort.Comment, "");
            ViewBag.ListSort = ListSort;

            return View();
        }

        public ActionResult GetModels(Pagination pagination, int SortID, string keyword)
        {
            PagedData<News> pagedData = this.Service.GetPageData(pagination, SortID,-1,keyword);
            return this.SuccessData(pagedData);
        }

        public ActionResult GetModels2(Pagination pagination, int SortID, string keyword)
        {
            PagedData<News> pagedData = this.Service.GetCommentPageData(pagination, SortID, -1, keyword);
            return this.SuccessData(pagedData);
        }

        [Permission("wiki.news.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        { 
            this.Service.Delete(id);
            return this.SuccessMsg("删除成功");
        }

        [HttpGet]
        public ActionResult Add()
        {

            ISortService SortService = this.CreateService<ISortService>();
            List<Sort> ListSort = SortService.GetList((int)EnumSort.News, "");
            ViewBag.ListSort = ListSort;

            return View();
        }

        public ActionResult Detail(string Id = "")
        {
            if (string.IsNullOrEmpty(Id))
            {
                return Redirect("Index");
            }
            else
            {
                News modle = this.Service.GetModel(Id);
                if (modle == null)
                {
                    Response.Redirect("/Wiki/News/Index");
                }
                ISortService SortService = this.CreateService<ISortService>();
                List<Sort> ListSort = SortService.GetList((int)EnumSort.News, "");
                ViewBag.ListSort = ListSort;
                ViewBag.NewsModle = modle;
            }


            return View();
        }

        [HttpGet]
        public ActionResult Edit(string Id="")
        {

            if (string.IsNullOrEmpty(Id))
            {
                return Redirect("Index");
            }
            else
            {
                News modle = this.Service.GetModel(Id);
                if (modle == null)
                {
                    //RedirectToAction("News", "Index");
                    Response.Redirect("/Wiki/News/Index");
                }
                

                ISortService SortService = this.CreateService<ISortService>();
                List<Sort> ListSort = SortService.GetList((int)EnumSort.News, "");
                ViewBag.ListSort = ListSort;

                ViewBag.NewsModle = modle;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit2(string Id = "")
        {

            if (string.IsNullOrEmpty(Id))
            {
                return Redirect("Comment");
            }
            else
            {
                News modle = this.Service.GetModel(Id);
                if (modle == null)
                {
                    //RedirectToAction("News", "Index");
                    Response.Redirect("/Wiki/News/Comment");
                }


                ISortService SortService = this.CreateService<ISortService>();
                List<Sort> ListSort = SortService.GetList((int)EnumSort.Comment, "");
                ViewBag.ListSort = ListSort;

                ViewBag.NewsModle = modle;
            }
            return View();
        }


        [HttpPost]
        public ActionResult Edit(UpdateNewsInput modle)
        {
            modle.CreateDate = DateTime.Now;
            modle.CreateID = this.CurrentSession.UserId;
            this.Service.Update(modle);
             
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit2(UpdateNewsInput modle)
        {
            modle.CreateDate = DateTime.Now;
            modle.CreateID = this.CurrentSession.UserId;
            this.Service.Update(modle);

            return RedirectToAction("Comment");
        }

        [HttpPost]
        public ActionResult Add(AddNewsInput modle)
        {
            modle.CreateDate = DateTime.Now;
            modle.CreateID = this.CurrentSession.UserId;
            this.Service.Add(modle);
                          
            return RedirectToAction("Index");
        }

    }
}