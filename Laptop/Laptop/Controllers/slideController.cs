using Laptop.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class SlideController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: slide
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var slide = from b in _db.Slides
                        select b;
            ViewBag.slide = slide;
            return View(slide.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var slide = (from b in _db.Slides
                         orderby b.ID descending
                         select b).Take(3);
            return View(slide);
        }

        [HttpPost]
        public ActionResult Create(Slide slide)
        {
            var test = _db.Slides.FirstOrDefault(p => p.Title == Request["Ten"]);
            ViewBag.date = DateTime.Now;
            if (test != null)
            {
                ViewBag.test = "Tiêu đề đã tồn tại!";
            }
            else
            {
                slide.Title = Request["Ten"];
                slide.Action = Request["action"];
                slide.Content = Request["content"];
                slide.Discount = Convert.ToInt32(Request["discount"]);
                slide.Image = Request["Anh"];
                slide.created_at = ViewBag.date;
                _db.Slides.Add(slide);
                _db.SaveChanges();
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var slide = _db.Slides.First(b => b.ID == id);
            return View(slide);
        }

        [HttpPost]
        public ActionResult Edit(int id, Slide slide)
        {
            ViewBag.date = DateTime.Now;
            slide = _db.Slides.SingleOrDefault(b => b.ID == id);
            if (slide != null)
            {
                slide.Title = Request["Ten"];
                slide.Action = Request["action"];
                slide.Content = Request["content"];
                slide.Discount = Convert.ToInt32(Request["discount"]);
                slide.Image = Request["Anh"];
                slide.updated_at = ViewBag.date;
                _db.Entry(slide).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var slide = _db.Slides.SingleOrDefault(b => b.ID == id);
            if (slide != null) _db.Slides.Remove(slide);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit_cre(int id)
        {
            var slide = _db.Slides.First(b => b.ID == id);
            return View(slide);
        }

        [HttpPost]
        public ActionResult Edit_cre(int id, Slide slide)
        {
            ViewBag.date = DateTime.Now;
            slide = _db.Slides.SingleOrDefault(b => b.ID == id);
            if (slide != null)
            {
                slide.Title = Request["Ten"];
                slide.Action = Request["action"];
                slide.Content = Request["content"];
                slide.Discount = Convert.ToInt32(Request["discount"]);
                slide.Image = Request["Anh"];
                slide.updated_at = ViewBag.date;
                _db.Entry(slide).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Create");
        }

        public ActionResult Delete_cre(int id)
        {
            var slide = _db.Slides.SingleOrDefault(b => b.ID == id);
            if (slide != null) _db.Slides.Remove(slide);
            _db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}