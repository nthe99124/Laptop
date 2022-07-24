using Laptop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class ClientFavoriteListController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: _clientFavoriteList
        public ActionResult Index()
        {
            var idCus = Convert.ToInt32(Session["ID_cus"]);
            if (Session["user"] == null)
            {
                ViewBag.fl = null;
            }
            else
            {
                ViewBag.fl = from c in _db.Favorites_list
                             join pro in _db.Products on c.ID_Product equals pro.ID
                             where c.ID_Customer == idCus
                             orderby c.created_at descending
                             select new GioHang
                             {
                                 ID = c.ID,
                                 ID_pro = pro.ID,
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
            var key = Convert.ToInt32(Request["key"]);
            ViewBag.date = DateTime.Now;
            var fa = _db.Favorites_list.SingleOrDefault(a => a.ID_Product == key);
            if (Session["user"] != null)
            {
                if (fa != null)
                {
                    fa.created_at = ViewBag.date;
                    _db.Entry(fa).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    fal.ID_Product = key;
                    fal.ID_Customer = Convert.ToInt32(Session["ID_cus"]);
                    fal.created_at = ViewBag.date;
                    _db.Favorites_list.Add(fal);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "ClientProductDetail", new { key = Convert.ToInt32(Session["load"]) });
        }

        public ActionResult Delete(Favorites_list fa)
        {
            var key = Convert.ToInt32(Request["key"]);
            fa = _db.Favorites_list.SingleOrDefault(c => c.ID == key);
            if (fa != null) _db.Favorites_list.Remove(fa);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}