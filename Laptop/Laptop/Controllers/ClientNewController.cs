using Laptop.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Laptop.Controllers
{
    public class ClientNewController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: _clientNew
        public ActionResult Index(int? page)
        {
            var news = from n in _db.News
                       orderby n.ID descending
                       select n;
            return View(news.ToPagedList(page ?? 1, 7));
        }

        public ActionResult NewDetail()
        {
            var key = Convert.ToInt32(Request["key"]);
            var news = from n in _db.News
                       where n.ID == key
                       select n;
            ViewBag.n = _db.News.OrderByDescending(tbl => tbl.ID).Skip(0).Take(6).ToList();
            return View(news);
        }
    }
}