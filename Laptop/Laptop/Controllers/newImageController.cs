using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;

namespace Laptop.Controllers
{
    public class newImageController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: newImage
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            return View();
        }
        public ActionResult Image(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var image = from p in db.New_Images
                        orderby p.ID_New
                        select p;
            ViewBag.ni = image;

            ViewBag.news = from b in db.News
                              select b;
            ViewBag.image = (from p in db.New_Images
                             orderby p.ID descending
                             select p).Take(3);
            return View(image.ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Image(int? page, New_Image iimgae)
        {

            var image = from p in db.New_Images
                        orderby p.ID_New
                        select p;
            ViewBag.news = from b in db.News
                              select b;
            ViewBag.image = (from p in db.New_Images
                             orderby p.ID descending
                             select p).Take(3);
            New test = db.News.Where(p => p.ID == Convert.ToInt32(Request["ID_Tin"])).FirstOrDefault();
            ViewBag.date = DateTime.Now;
            if (test == null)
            {
                ViewBag.test = "Tin có ID " + Request["ID_Tin"] + " không tồn tại!";
            }
            else
            {
                iimgae.ID_New = Convert.ToInt32(Request["ID_Tin"]);
                iimgae.Image = Request["Anh"];
                iimgae.created_at = ViewBag.date;
                db.New_Images.InsertOnSubmit(iimgae);
                db.SubmitChanges();
            }
            return View(image.ToPagedList(page ?? 1, 5));
        }
        /*public ActionResult Search(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var image = from p in db.New_Images
                        orderby p.ID
                        select p;
            ViewBag.news = from b in db.News
                              select b;
            ViewBag.image = (from p in db.New_Images
                             orderby p.ID descending
                             select p).Take(3);
            return View(image.ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Search(int? page, New_Image a)
        {
            int id = Convert.ToInt32(Request["key"]);
            *//*var image = from p in db.New_Images
                        select p;*//*
            ViewBag.news = from b in db.News
                              select b;
            ViewBag.image = (from p in db.New_Images
                             orderby p.ID descending
                             select p).Take(3);
            var image = from p in db.New_Images
                        where p.ID_New == id
                        select p;
            return View(image.ToPagedList(page ?? 1, 5));
        }*/

        public ActionResult Delete_img(int id)
        {
            var img = db.New_Images.Where(b => b.ID == id).SingleOrDefault();
            db.New_Images.DeleteOnSubmit(img);
            db.SubmitChanges();
            return RedirectToAction("Image");
        }
    }
}