using Laptop.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class ProductController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: Product
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var pro = from p in _db.Products
                      orderby p.Name descending
                      select p;
            ViewBag.product = from b in _db.Products
                              select b;
            return View(pro.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var pro = (from p in _db.Products
                       orderby p.ID descending
                       select p).Take(3);
            ViewBag.brand = from p in _db.Brands
                            select p;
            return View(pro);
        }

        [HttpPost]
        public ActionResult Create(Product pro)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            ViewBag.date = DateTime.Now;
            var name = Request["Ten"];
            var test = _db.Products.FirstOrDefault(p => p.Name == name);
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
                _db.Products.Add(pro);
                _db.SaveChanges();
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var pro = _db.Products.First(b => b.ID == id);
            ViewBag.brand = from p in _db.Brands
                            select p;
            return View(pro);
        }

        [HttpPost]
        public ActionResult Edit(int id, Product pro)
        {
            ViewBag.date = DateTime.Now;
            pro = _db.Products.SingleOrDefault(p => p.ID == id);
            if (pro != null)
            {
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
                _db.Entry(pro).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var pro = _db.Products.SingleOrDefault(b => b.ID == id);
            if (pro != null) _db.Products.Remove(pro);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit_cre(int id)
        {
            var pro = _db.Products.First(b => b.ID == id);
            ViewBag.brand = from p in _db.Brands
                            select p;
            return View(pro);
        }

        [HttpPost]
        public ActionResult Edit_cre(int id, Product pro)
        {
            ViewBag.date = DateTime.Now;
            pro = _db.Products.SingleOrDefault(p => p.ID == id);
            if (pro != null)
            {
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
                _db.Entry(pro).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Create");
        }

        public ActionResult Delete_cre(int id)
        {
            var pro = _db.Products.SingleOrDefault(b => b.ID == id);
            if (pro != null) _db.Products.Remove(pro);
            _db.SaveChanges();
            return RedirectToAction("Create");
        }

        public ActionResult Test()
        {
            var pro = from p in _db.Products
                      select p;
            ViewBag.product = from b in _db.Products
                              select b;
            return View(pro);
        }
    }
}