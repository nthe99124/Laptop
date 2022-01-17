using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;

namespace Laptop.Controllers
{
    public class slideController : Controller
    {
        // GET: slide
        laptopDataContext db = new laptopDataContext();
        
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var slide = from b in db.Slides
                        select b;
            ViewBag.slide = slide;
            return View(slide.ToPagedList(page ?? 1, 5));
        }/*
        [HttpPost]
        public ActionResult Index(int? page, FormCollection a)
        {

            string name = Request["key"];
            var slide = from b in db.Slides
                        where b.Title.Contains(name)
                        select b;
            return View(slide.ToPagedList(page ?? 1, 5));
        }*/
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var slide = (from b in db.Slides
                         orderby b.ID descending
                         select b).Take(3);
            return View(slide);
        }
        [HttpPost]
        public ActionResult Create(Slide slide)
        {
            Slide test = db.Slides.Where(p => p.Title == Request["Ten"]).FirstOrDefault();
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
                db.Slides.InsertOnSubmit(slide);
                db.SubmitChanges();
            }

            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var slide = db.Slides.First(b => b.ID == id);
            return View(slide);
        }
        [HttpPost]
        public ActionResult Edit(int id, Slide slide)
        {
            /*var filename = Request["anh"];*/
            ViewBag.date = DateTime.Now;
            slide = db.Slides.Where(b => b.ID == id).SingleOrDefault();
            slide.Title = Request["Ten"];
            slide.Action = Request["action"];
            slide.Content = Request["content"];
            slide.Discount = Convert.ToInt32(Request["discount"]);
            slide.Image = Request["Anh"];
            slide.updated_at = ViewBag.date;
            /*
            slide.Image = "/Content/slide/Image/" + GetFileName(file.FileName);*/
            UpdateModel(slide);
            db.SubmitChanges();
            return RedirectToAction("Index");

            return this.Edit(id);
        }
        public ActionResult Delete(int id)
        {
            var slide = db.Slides.Where(b => b.ID == id).SingleOrDefault();
            db.Slides.DeleteOnSubmit(slide);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit_cre(int id)
        {
            var slide = db.Slides.First(b => b.ID == id);
            return View(slide);
        }
        [HttpPost]
        public ActionResult Edit_cre(int id, Slide slide)
        {
            /*var filename = Request["anh"];*/
            ViewBag.date = DateTime.Now;
            slide = db.Slides.Where(b => b.ID == id).SingleOrDefault();
            slide.Title = Request["Ten"];
            slide.Action = Request["action"];
            slide.Content = Request["content"];
            slide.Discount = Convert.ToInt32(Request["discount"]);
            slide.Image = Request["Anh"];
            slide.updated_at = ViewBag.date;
            UpdateModel(slide);
            db.SubmitChanges();
            return RedirectToAction("Create");

            return this.Edit(id);
        }
        public ActionResult Delete_cre(int id)
        {
            var slide = db.Slides.Where(b => b.ID == id).SingleOrDefault();
            db.Slides.DeleteOnSubmit(slide);
            db.SubmitChanges();
            return RedirectToAction("Create");
        }
    }
}