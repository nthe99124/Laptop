using Laptop.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class ProductColorController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: productColor
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            return View();
        }

        public ActionResult Color(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var color = from p in _db.Product_Color
                        select p;
            ViewBag.c = from p in _db.Product_Color
                        join pro in _db.Products on p.ID_Product equals pro.ID
                        join co in _db.Colorrs on p.ID_Color equals co.ID
                        select new mau
                        {
                            ID = p.ID,
                            ID_Color = (int)p.ID_Color,
                            ID_Product = (int)p.ID_Product,
                            Quantity = (int)p.Quantity,
                            Color = co.Color,
                            Name = pro.Name,
                        };
            ViewBag.product = from b in _db.Products
                              select b;
            ViewBag.co = from b in _db.Colorrs
                         select b;
            ViewBag.color = (from p in _db.Product_Color
                             join pro in _db.Products on p.ID_Product equals pro.ID
                             join co in _db.Colorrs on p.ID_Color equals co.ID
                             orderby p.ID descending
                             select new mau
                             {
                                 ID = p.ID,
                                 ID_Color = (int)p.ID_Color,
                                 ID_Product = (int)p.ID_Product,
                                 Quantity = (int)p.Quantity,
                                 Color = co.Color,
                                 Name = pro.Name,
                             }).Take(3);
            return View(color.ToPagedList(page ?? 1, 5));
        }

        [HttpPost]
        public ActionResult Color(int? page, Product_Color productColor)
        {
            ViewBag.product = from b in _db.Products
                              select b;
            ViewBag.co = from b in _db.Colorrs
                         select b;
            ViewBag.c = from p in _db.Product_Color
                        join pro in _db.Products on p.ID_Product equals pro.ID
                        join co in _db.Colorrs on p.ID_Color equals co.ID
                        select new mau
                        {
                            ID = p.ID,
                            ID_Color = (int)p.ID_Color,
                            ID_Product = (int)p.ID_Product,
                            Quantity = (int)p.Quantity,
                            Color = co.Color,
                            Name = pro.Name,
                        };
            ViewBag.color = (from p in _db.Product_Color
                             join pro in _db.Products on p.ID_Product equals pro.ID
                             join co in _db.Colorrs on p.ID_Color equals co.ID
                             orderby p.ID descending
                             select new mau
                             {
                                 ID = p.ID,
                                 ID_Color = (int)p.ID_Color,
                                 ID_Product = (int)p.ID_Product,
                                 Quantity = (int)p.Quantity,
                                 Color = co.Color,
                                 Name = pro.Name,
                             }).Take(3);
            var test = _db.Product_Color.FirstOrDefault(p => p.ID_Product == Convert.ToInt32(Request["ID_SP"])
                                                             && p.ID_Color == Convert.ToInt32(Request["Mau"]));
            ViewBag.date = DateTime.Now;

            if (test != null)
            {
                test.Quantity += Convert.ToInt32(Request["SL"]);
                _db.Entry(test).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Color", "ProductColor");
            }
            else
            {
                productColor.ID_Product = Convert.ToInt32(Request["ID_SP"]);
                productColor.ID_Color = Convert.ToInt32(Request["Mau"]);
                productColor.Quantity = Convert.ToInt32(Request["SL"]);
                productColor.created_at = ViewBag.date;
                _db.Product_Color.Add(productColor);
                _db.SaveChanges();
                return RedirectToAction("Color", "ProductColor");
            }
        }

        public ActionResult Delete_co(int id)
        {
            var color = _db.Product_Color.SingleOrDefault(b => b.ID == id);
            if (color != null) _db.Product_Color.Remove(color);
            _db.SaveChanges();
            return RedirectToAction("Color");
        }
    }
}