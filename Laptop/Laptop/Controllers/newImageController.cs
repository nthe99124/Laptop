using Laptop.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class NewImageController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: newImage
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            return View();
        }

        public ActionResult Image(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var image = from p in _db.New_Image
                        orderby p.ID_New
                        select p;
            ViewBag.ni = image;

            ViewBag.news = from b in _db.News
                           select b;
            ViewBag.image = (from p in _db.New_Image
                             orderby p.ID descending
                             select p).Take(3);
            return View(image.ToPagedList(page ?? 1, 5));
        }

        [HttpPost]
        public ActionResult Image(int? page, New_Image newImage)
        {

            var image = from p in _db.New_Image
                        orderby p.ID_New
                        select p;
            ViewBag.news = from b in _db.News
                           select b;
            ViewBag.image = (from p in _db.New_Image
                             orderby p.ID descending
                             select p).Take(3);
            var test = _db.News.FirstOrDefault(p => p.ID == Convert.ToInt32(Request["ID_Tin"]));
            ViewBag.date = DateTime.Now;
            if (test == null)
            {
                ViewBag.test = "Tin có ID " + Request["ID_Tin"] + " không tồn tại!";
            }
            else
            {
                newImage.ID_New = Convert.ToInt32(Request["ID_Tin"]);
                newImage.Image = Request["Anh"];
                newImage.created_at = ViewBag.date;
                _db.New_Image.Add(newImage);
                _db.SaveChanges();
            }
            return View(image.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Delete_img(int id)
        {
            var img = _db.New_Image.Where(b => b.ID == id).SingleOrDefault();
            if (img != null) _db.New_Image.Remove(img);
            _db.SaveChanges();
            return RedirectToAction("Image");
        }
    }
}