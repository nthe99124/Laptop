using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;


namespace Laptop.Controllers
{
    public class brandController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: brand
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var brand = from b in db.Brands 
                        select b;
            ViewBag.brand = brand;
            return View(brand.ToPagedList(page ?? 1, 5));
        }/*
        [HttpPost]
        public ActionResult Index(int? page, FormCollection a)
        {
            string name = Request["key"]; 
            var brand = from b in db.Brands
                        where b.Name.Contains(name)
                        select b;
            return View(brand.ToPagedList(page ?? 1, 5));
        }*/
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            var brand = (from b in db.Brands
                        orderby b.ID descending
                        select b).Take(3);
            return View(brand);
        }
        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            
            Brand test = db.Brands.Where(p => p.Name == Request["Ten"]).FirstOrDefault();
            ViewBag.date = DateTime.Now;
            if (test != null)
            {
                ViewBag.test = "Hãng " + Request["Ten"] + " đã tồn tại!";
            }
            else
            {
                brand.Name = Request["Ten"];
                brand.Description = Request["MoTa"];
                brand.Image = Request["Anh"];
                brand.created_at = ViewBag.date;
                db.Brands.InsertOnSubmit(brand);
                db.SubmitChanges();
            }

            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var brand = db.Brands.First(b => b.ID == id);
            return View(brand);
        }
        [HttpPost]
        public ActionResult Edit(int id, Brand brand)
        {
            /*var filename = Request["anh"];*/
            ViewBag.date = DateTime.Now;
            brand = db.Brands.Where(b => b.ID == id).SingleOrDefault();
            /*brand.Name = Request["Ten"];*/
            brand.Description = Request["MoTa"];
            brand.Image = Request["logo"];
            brand.updated_at = ViewBag.date;
            /*
            brand.Image = "/Content/Brand/Image/" + GetFileName(file.FileName);*/
            UpdateModel(brand);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var brand = db.Brands.Where(b => b.ID == id).SingleOrDefault();
            db.Brands.DeleteOnSubmit(brand);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit_cre(int id)
        {
            var brand = db.Brands.First(b => b.ID == id);
            return View(brand);
        }
        [HttpPost]
        public ActionResult Edit_cre(int id, Brand brand)
        {
            /*var filename = Request["anh"];*/
            ViewBag.date = DateTime.Now;
            brand = db.Brands.Where(b => b.ID == id).SingleOrDefault();
            /*brand.Name = Request["Ten"];*/
            brand.Description = Request["MoTa"];
            brand.Image = Request["logo"];
            brand.updated_at = ViewBag.date;
            /*
            brand.Image = "/Content/Brand/Image/" + GetFileName(file.FileName);*/
            UpdateModel(brand);
            db.SubmitChanges();
            return RedirectToAction("Create");

            return this.Edit(id);
        }
    }
}