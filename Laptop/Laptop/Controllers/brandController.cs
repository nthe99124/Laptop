using Laptop.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class BrandController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: Brand
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var brand = from b in _db.Brands
                        orderby b.Name descending
                        select b;
            ViewBag.brand = brand;
            return View(brand.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var brand = (from b in _db.Brands
                         orderby b.ID descending
                         select b).Take(3);
            return View(brand);
        }

        [HttpPost]
        public ActionResult Create(Brand brand)
        {

            var test = _db.Brands.FirstOrDefault(p => p.Name == Request["Ten"]);
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
                _db.Brands.Add(brand);
                _db.SaveChanges();
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var brand = _db.Brands.First(b => b.ID == id);
            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit(int id, Brand brand)
        {
            ViewBag.date = DateTime.Now;
            brand = _db.Brands.SingleOrDefault(b => b.ID == id);
            if (brand != null)
            {
                brand.Description = Request["MoTa"];
                brand.Image = Request["logo"];
                brand.updated_at = ViewBag.date;
                /*
            Brand.Image = "/Content/Brand/Image/" + GetFileName(file.FileName);*/
                _db.Entry(brand).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var brand = _db.Brands.SingleOrDefault(b => b.ID == id);
            if (brand != null) _db.Brands.Remove(brand);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit_cre(int id)
        {
            var brand = _db.Brands.First(b => b.ID == id);
            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit_cre(int id, Brand brand)
        {
            ViewBag.date = DateTime.Now;
            brand = _db.Brands.SingleOrDefault(b => b.ID == id);
            if (brand != null)
            {
                brand.Description = Request["MoTa"];
                brand.Image = Request["logo"];
                brand.updated_at = ViewBag.date;
                _db.Entry(brand).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}