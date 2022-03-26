using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;

namespace Laptop.Controllers
{
    public class _clientFavoriteListController : Controller
    {
        LaptopNTT db = new LaptopNTT();
        // GET: _clientFavoriteList
        public ActionResult Index()
        {
            var IDcus = Convert.ToInt32(Session["ID_cus"]);
            if (Session["user"] == null)
            {
                ViewBag.fl = null;
            }
            else
            {
                ViewBag.fl = from c in db.Favorites_list
                             join pro in db.Products on c.ID_Product equals pro.ID
                             where c.ID_Customer == IDcus
                             orderby c.created_at descending
                             select new GioHang
                             {
                                 ID = (int)c.ID,
                                 ID_pro = (int)pro.ID,
                                 Image = pro.Image,
                                 Name = pro.Name,
                                 Description = pro.Description,
                                 Price = (int)pro.Promotion_Price
                             };
            }
            return View();
        }
        public ActionResult Add(Favorites_list fal)
        {
            int key = Convert.ToInt32(Request["key"]);
            ViewBag.date = DateTime.Now;
            Favorites_list fa = db.Favorites_list.Where(a => a.ID_Product == key).SingleOrDefault();
            if (Session["user"] == null)
            {
            }
            else if (fa != null)
            {
                fa.created_at = ViewBag.date;
                db.Entry(fa).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                fal.ID_Product = key;
                fal.ID_Customer = Convert.ToInt32(Session["ID_cus"]);
                fal.created_at = ViewBag.date;
                db.Favorites_list.Add(fal);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "_clientProductDetail", new { key = Convert.ToInt32(Session["load"]) });


        }
        public ActionResult Delete(Favorites_list fa)
        {
            int key = Convert.ToInt32(Request["key"]);
            fa = db.Favorites_list.Where(c => c.ID == key).SingleOrDefault();
            db.Favorites_list.Remove(fa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}