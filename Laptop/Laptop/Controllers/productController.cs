using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
namespace Laptop.Controllers
{
    public class productController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: product
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var pro = from p in db.Products 
                      select p;
            ViewBag.product = from b in db.Products
                              select b;
            return View(pro.ToPagedList(page ?? 1, 5));
        }/*
        [HttpPost]
        public ActionResult Index(int? page, FormCollection a)
        {
            string name = Request["key"];
            var pro = from p in db.Products
                      where p.Name.Contains(name)
                      select p;
            return View(pro.ToPagedList(page ?? 1, 5));
        }*/


        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var pro = (from p in db.Products
                      orderby p.ID descending
                      select p).Take(3);
            ViewBag.brand = from p in db.Brands
                  select p;
            return View(pro);
        }
        [HttpPost]
        public ActionResult Create( Product pro)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            ViewBag.date = DateTime.Now;
            Product test = db.Products.Where(p => p.Name == Request["Ten"]).FirstOrDefault();
            if (test != null)
            {
                ViewBag.test = "Sản phẩm " + Request["Ten"] + " đã tồn tại!";
            }
            else
            {
                pro.Name = Request["Ten"];
                pro.Description = Request["MoTa"];
                pro.Image = Request["Anh"];
                pro.Warranty = Request["Warr"];
                pro.Group_Pro = Request["Phanloai"];
                pro.ID_Brand = Convert.ToInt32(Request["ID_Hang"]);
                pro.Price = Convert.ToInt32(Request["Pri"]);
                pro.Promotion_Price = Convert.ToInt32(Request["Pro_Price"]);
                pro.CPU = Request["CPU"];
                pro.GPU = Request["GPU"];
                pro.RAM = Request["RAM"];
                pro.ROM = Request["ROM"];
                pro.Pin = Request["pin"];
                pro.Weight = Request["weight"];
                pro.Size = Request["size"];
                pro.Monitor = Request["Moni"];
                pro.Operating = Request["Ope"];
                pro.created_at = ViewBag.date;
                db.Products.InsertOnSubmit(pro);
                db.SubmitChanges();
            }

            return this.Create();
        }


        public ActionResult Edit(int id)
        {
            var pro = db.Products.First(b => b.ID == id);
            ViewBag.brand = from p in db.Brands
                            select p;
            return View(pro);
        }
        [HttpPost]
        public ActionResult Edit(int id, Product pro)
        {
            ViewBag.date = DateTime.Now;
            pro = db.Products.Where(p => p.ID == id).SingleOrDefault();
            pro.Name = Request["Ten"];
            pro.Description = Request["MoTa"];
            pro.Warranty = Request["Warr"];
            pro.Image = Request["Anh"];
            pro.Group_Pro = Request["Phanloai"];
            pro.ID_Brand = Convert.ToInt32(Request["ID_Hang"]);
            pro.Price = Convert.ToDecimal(Request["Pri"]);
            pro.Promotion_Price = Convert.ToDecimal(Request["Pro_Price"]);
            pro.CPU = Request["CPU"];
            pro.GPU = Request["GPU"];
            pro.RAM = Request["RAM"];
            pro.ROM = Request["ROM"];
            pro.Pin = Request["pin"];
            pro.Weight = Request["weight"];
            pro.Size = Request["size"];
            pro.Monitor = Request["Moni"];
            pro.Operating = Request["Ope"];
            pro.updated_at = ViewBag.date;
            UpdateModel(pro);
            db.SubmitChanges();
            return RedirectToAction("Index");

            return this.Edit(id);
        }


        public ActionResult Delete(int id)
        {
            var pro = db.Products.Where(b => b.ID == id).SingleOrDefault();
            db.Products.DeleteOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit_cre(int id)
        {
            var pro = db.Products.First(b => b.ID == id);
            ViewBag.brand = from p in db.Brands
                            select p;
            return View(pro);
        }
        [HttpPost]
        public ActionResult Edit_cre(int id, Product pro)
        {
            ViewBag.date = DateTime.Now;
            pro = db.Products.Where(p => p.ID == id).SingleOrDefault();
            pro.Name = Request["Ten"];
            pro.Description = Request["MoTa"];
            pro.Warranty = Request["Warr"];
            pro.Image = Request["Anh"];
            pro.Group_Pro = Request["Phanloai"];
            pro.ID_Brand = Convert.ToInt32(Request["ID_Hang"]);
            pro.Price = Convert.ToDecimal(Request["Pri"]);
            pro.Promotion_Price = Convert.ToDecimal(Request["Pro_Price"]);
            pro.CPU = Request["CPU"];
            pro.GPU = Request["GPU"];
            pro.RAM = Request["RAM"];
            pro.ROM = Request["ROM"];
            pro.Pin = Request["pin"];
            pro.Weight = Request["weight"];
            pro.Size = Request["size"];
            pro.Monitor = Request["Moni"];
            pro.Operating = Request["Ope"];
            pro.updated_at = ViewBag.date;
            UpdateModel(pro);
            db.SubmitChanges();
            return RedirectToAction("Create");

            return this.Edit_cre(id);
        }


        public ActionResult Delete_cre(int id)
        {
            var pro = db.Products.Where(b => b.ID == id).SingleOrDefault();
            db.Products.DeleteOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("Create");
        }
        public ActionResult test()
        {
            /*if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }*/
            var pro = from p in db.Products
                      select p;
            ViewBag.product = from b in db.Products
                              select b;
            return View(pro);
        }
    }
}