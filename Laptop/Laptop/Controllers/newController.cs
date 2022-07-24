using Laptop.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class NewController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();
        // GET: new
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var tin = from b in _db.News
                      orderby b.Title descending
                      select b;
            ViewBag.tin = tin;
            return View(tin.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var tin = (from b in _db.News
                       orderby b.ID descending
                       select b).Take(3);
            return View(tin);
        }

        [HttpPost]
        public ActionResult Create(News news)
        {
            var test = _db.News.FirstOrDefault(p => p.Title == Request["Title"]);
            ViewBag.date = DateTime.Now;
            if (test != null)
            {
                ViewBag.test = "Tiêu đề " + Request["Title"] + " đã tồn tại!";
            }
            else
            {
                news.Title = Request["Title"];
                news.Content = Request["Content"];
                news.Image = Request["Anh"];
                news.created_at = ViewBag.date;
                _db.News.Add(news);
                _db.SaveChanges();
            }

            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var tin = _db.News.First(b => b.ID == id);
            return View(tin);
        }

        [HttpPost]
        public ActionResult Edit(int id, News tin)
        {
            ViewBag.date = DateTime.Now;
            tin = _db.News.SingleOrDefault(b => b.ID == id);
            tin.Title = Request["Title"];
            tin.Content = Request["Content"];
            tin.Image = Request["Anh"];
            tin.updated_at = ViewBag.date;
            _db.Entry(tin).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit_cre(int id)
        {
            var tin = _db.News.First(b => b.ID == id);
            return View(tin);
        }

        [HttpPost]
        public ActionResult Edit_cre(int id, News tin)
        {
            ViewBag.date = DateTime.Now;
            tin = _db.News.SingleOrDefault(b => b.ID == id);
            tin.Title = Request["Title"];
            tin.Content = Request["Content"];
            tin.Image = Request["Anh"];
            tin.updated_at = ViewBag.date;
            _db.Entry(tin).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Create");
        }

        public ActionResult Delete(int id)
        {
            var tin = _db.News.SingleOrDefault(b => b.ID == id);
            _db.News.Remove(tin);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete_cre(int id)
        {
            var tin = _db.News.SingleOrDefault(b => b.ID == id);
            _db.News.Remove(tin);
            _db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}