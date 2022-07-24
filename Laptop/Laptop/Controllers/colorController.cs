using Laptop.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
namespace Laptop.Controllers
{
    public class ColorController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: color
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var color = from p in _db.Colorrs
                        select p;
            ViewBag.co = color;
            ViewBag.color = (from p in _db.Colorrs
                             orderby p.ID descending
                             select p).Take(3);
            return View(color.ToPagedList(page ?? 1, 5));
        }

        [HttpPost]
        public ActionResult Index(int? page, Colorr ccolor)
        {
            var color = from p in _db.Colorrs
                        select p;
            ViewBag.color = (from p in _db.Colorrs
                             orderby p.ID descending
                             select p).Take(3);
            var test = _db.Colorrs.FirstOrDefault(p => p.Color == Request["Mau"]);

            ViewBag.date = DateTime.Now;

            if (test != null)
            {
                ViewBag.test = "Màu " + Request["Mau"] + " đã tồn tại!";
            }
            else
            {
                ccolor.Color = Request["Mau"];
                ccolor.Image = Request["Anh"];
                ccolor.created_at = ViewBag.date;
                _db.Colorrs.Add(ccolor);
                _db.SaveChanges();
                return RedirectToAction("Index", "Color");
            }
            return View(color.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Delete(int id)
        {
            var color = _db.Colorrs.SingleOrDefault(b => b.ID == id);
            if (color != null) _db.Colorrs.Remove(color);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var color = _db.Colorrs.First(b => b.ID == id);
            return View(color);
        }

        [HttpPost]
        public ActionResult Edit(int id, Colorr color)
        {
            ViewBag.date = DateTime.Now;
            color = _db.Colorrs.SingleOrDefault(b => b.ID == id);
            if (color != null)
            {
                color.Color = Request["color"];
                color.Image = Request["logo"];
                color.updated_at = ViewBag.date;
                _db.Entry(color).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}