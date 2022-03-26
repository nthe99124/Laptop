using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using System.Data;
using System.Data.Entity;

namespace Laptop.Controllers
{
    public class newController : Controller
    {
        LaptopNTT db = new LaptopNTT();
        // GET: new
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var tin = from b in db.News
                      orderby b.Title descending
                      select b;
            ViewBag.tin = tin;
            return View(tin.ToPagedList(page ?? 1, 5));
        }
        /*[HttpPost]
        public ActionResult Index(int? page, FormCollection a)
        {
            string title = Request["key"];
            var tin = from b in db.News
                        where b.Title.Contains(title)
                        select b;
            return View(tin.ToPagedList(page ?? 1, 5));
        }*/
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var tin = (from b in db.News
                       orderby b.ID descending
                       select b).Take(3);
            return View(tin);
        }
        [HttpPost]
        public ActionResult Create(News tin)
        {
            News test = db.News.Where(p => p.Title == Request["Title"]).FirstOrDefault();
            ViewBag.date = DateTime.Now;
            if (test != null)
            {
                ViewBag.test = "Tiêu đề " + Request["Title"] + " đã tồn tại!";
            }
            else
            {
                tin.Title = Request["Title"];
                tin.Content = Request["Content"];
                tin.Image = Request["Anh"];
                tin.created_at = ViewBag.date;
                db.News.Add(tin);
                db.SaveChanges();
            }

            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var tin = db.News.First(b => b.ID == id);
            return View(tin);
        }
        [HttpPost]
        public ActionResult Edit(int id, News tin)
        {
            /*var filename = Request["anh"];*/
            ViewBag.date = DateTime.Now;
            tin = db.News.Where(b => b.ID == id).SingleOrDefault();
            tin.Title = Request["Title"];
            tin.Content = Request["Content"];
            tin.Image = Request["Anh"];
            tin.updated_at = ViewBag.date;
            /*
            tin.Image = "/Content/tin/Image/" + GetFileName(file.FileName);*/
            db.Entry(tin).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

            return this.Edit(id);
        }
        public ActionResult Edit_cre(int id)
        {
            var tin = db.News.First(b => b.ID == id);
            return View(tin);
        }
        [HttpPost]
        public ActionResult Edit_cre(int id, News tin)
        {
            /*var filename = Request["anh"];*/
            ViewBag.date = DateTime.Now;
            tin = db.News.Where(b => b.ID == id).SingleOrDefault();
            tin.Title = Request["Title"];
            tin.Content = Request["Content"];
            tin.Image = Request["Anh"];
            tin.updated_at = ViewBag.date;
            /*
            tin.Image = "/Content/tin/Image/" + GetFileName(file.FileName);*/
            db.Entry(tin).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Create");

            return this.Edit(id);
        }
        public ActionResult Delete(int id)
        {
            var tin = db.News.Where(b => b.ID == id).SingleOrDefault();
            db.News.Remove(tin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete_cre(int id)
        {
            var tin = db.News.Where(b => b.ID == id).SingleOrDefault();
            db.News.Remove(tin);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}