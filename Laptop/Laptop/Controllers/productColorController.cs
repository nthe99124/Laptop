using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using System.Data;
using System.Data.Entity;

namespace Laptop.Controllers
{
    public class productColorController : Controller
    {
        LaptopNTT db = new LaptopNTT();
        // GET: productColor
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            return View();
        }
        public ActionResult Color(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var color = from p in db.Product_Color
                        select p;
            ViewBag.c = from p in db.Product_Color
                        join pro in db.Products on p.ID_Product equals pro.ID
                        join co in db.Colorrs on p.ID_Color equals co.ID
                        select new mau
                        {
                            ID = p.ID,
                            ID_Color = (int)p.ID_Color,
                            ID_Product = (int)p.ID_Product,
                            Quantity = (int)p.Quantity,
                            Color = co.Color,
                            Name = pro.Name,
                        };
            ViewBag.product = from b in db.Products
                              select b;
            ViewBag.co = from b in db.Colorrs
                         select b;
            ViewBag.color = (from p in db.Product_Color
                             join pro in db.Products on p.ID_Product equals pro.ID
                             join co in db.Colorrs on p.ID_Color equals co.ID
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
        public ActionResult Color(int? page, Product_Color ccolor)
        {
            ViewBag.product = from b in db.Products
                              select b;
            ViewBag.co = from b in db.Colorrs
                         select b;
            var color = from p in db.Product_Color
                        select p;
            ViewBag.c = from p in db.Product_Color
                        join pro in db.Products on p.ID_Product equals pro.ID
                        join co in db.Colorrs on p.ID_Color equals co.ID
                        select new mau
                        {
                            ID = p.ID,
                            ID_Color = (int)p.ID_Color,
                            ID_Product = (int)p.ID_Product,
                            Quantity = (int)p.Quantity,
                            Color = co.Color,
                            Name = pro.Name,
                        };
            ViewBag.color = (from p in db.Product_Color
                             join pro in db.Products on p.ID_Product equals pro.ID
                             join co in db.Colorrs on p.ID_Color equals co.ID
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
            Product_Color test = db.Product_Color.Where(p => p.ID_Product == Convert.ToInt32(Request["ID_SP"])
                                                        && p.ID_Color == Convert.ToInt32(Request["Mau"])).FirstOrDefault();
            ViewBag.date = DateTime.Now;
            if (test != null)
            {
                /*ViewBag.test = "Sản phẩm có ID màu= " + Request["Mau"] + " đã tồn tại!";*/
                test.Quantity += Convert.ToInt32(Request["SL"]);
                db.Entry(test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Color", "productColor");
            }
            else
            {
                ccolor.ID_Product = Convert.ToInt32(Request["ID_SP"]);
                ccolor.ID_Color = Convert.ToInt32(Request["Mau"]);
                ccolor.Quantity = Convert.ToInt32(Request["SL"]);
                ccolor.created_at = ViewBag.date;
                db.Product_Color.Add(ccolor);
                db.SaveChanges();
                return RedirectToAction("Color", "productColor");
            }

            return View(color.ToPagedList(page ?? 1, 5));
        }


        /*public ActionResult Search_co(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var color = from p in db.Product_Colors
                        select p;
            ViewBag.product = from b in db.Products
                              select b;
            ViewBag.co = from b in db.Colorrs
                         select b;
            ViewBag.color = (from p in db.Product_Colors
                             orderby p.ID descending
                             select p).Take(3);
            return View(color.ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Search_co(int? page, Colorr a)
        {
            ViewBag.product = from b in db.Products
                              select b;
            ViewBag.co = from b in db.Colorrs
                         select b;
            ViewBag.color = (from p in db.Product_Colors
                             orderby p.ID descending
                             select p).Take(3);
            int id = Convert.ToInt32(Request["key"]);
            var color = from p in db.Product_Colors
                        where p.ID_Product == id
                        select p;
            return View(color.ToPagedList(page ?? 1, 5));
        }*/
        public ActionResult Delete_co(int id)
        {
            var color = db.Product_Color.Where(b => b.ID == id).SingleOrDefault();
            db.Product_Color.Remove(color);
            db.SaveChanges();
            return RedirectToAction("Color");
        }
    }
}