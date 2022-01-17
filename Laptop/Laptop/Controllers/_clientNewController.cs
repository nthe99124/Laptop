using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using PagedList;
namespace Laptop.Controllers
{
    public class _clientNewController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: _clientNew
        public ActionResult Index(int ?page)
        {
            var news = from n in db.News
                       orderby n.ID descending
                       select n;
            return View(news.ToPagedList(page ?? 1, 7));
        }
        public ActionResult NewDetail()
        {
            int key = Convert.ToInt32(Request["key"]);
            var news = from n in db.News
                       where n.ID ==key
                       select n;
            ViewBag.n= db.News.OrderByDescending(tbl => tbl.ID).Skip(0).Take(6).ToList();
            return View(news);
        }
    }
}