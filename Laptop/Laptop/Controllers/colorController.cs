using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using PagedList;
namespace Laptop.Controllers
{
    public class colorController : Controller
    {
        // GET: color
        laptopDataContext db = new laptopDataContext();
        public ActionResult Index(int ? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var color = from p in db.Colorrs                      
                      select p;
            ViewBag.co = color;
            ViewBag.color = (from p in db.Colorrs
                             orderby p.ID descending
                             select p).Take(3);
            return View(color.ToPagedList(page??1,5));
        }
        [HttpPost]
        public ActionResult Index(int? page, Colorr ccolor)
        {            
            var color = from p in db.Colorrs
                        select p;
            ViewBag.color = (from p in db.Colorrs
                             orderby p.ID descending
                             select p).Take(3);
            Colorr test = db.Colorrs.Where(p => p.Color == Request["Mau"]).FirstOrDefault();
            /*var test = (from p in db.Colorrs
                        where p.Color == Request["Mau"]
                        select p);*/

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
                db.Colorrs.InsertOnSubmit(ccolor);
                db.SubmitChanges();
                return RedirectToAction("Index", "color");
            }

            return View(color.ToPagedList(page ?? 1, 5));
        }

        /*public ActionResult Search(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var color = from p in db.Colorrs
                        select p;
            ViewBag.color = (from p in db.Colorrs
                             orderby p.ID descending
                             select p).Take(3);
            return View(color.ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Search(int? page, Colorr a)
        {
            ViewBag.color = (from p in db.Colorrs
                             orderby p.ID descending
                             select p).Take(3);
            string name = Request["key"];
            var color = from p in db.Colorrs
                        where p.Color.Contains(name)            
                        select p;       
            return View(color.ToPagedList(page ?? 1, 5));
        }*/
        public ActionResult Delete(int id)
        {
            var color = db.Colorrs.Where(b => b.ID == id).SingleOrDefault();
            db.Colorrs.DeleteOnSubmit(color);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var color = db.Colorrs.First(b => b.ID == id);
            return View(color);
        }
        [HttpPost]
        public ActionResult Edit(int id, Colorr colorr)
        {
            ViewBag.date = DateTime.Now;
            colorr = db.Colorrs.Where(b => b.ID == id).SingleOrDefault();
            colorr.Color = Request["color"];
            colorr.Image = Request["logo"];
            colorr.updated_at = ViewBag.date;
            UpdateModel(colorr);
            db.SubmitChanges();
            return RedirectToAction("Index");

            return this.Edit(id);
        }
    }
}