using Laptop.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: imageProduct
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            return View();
        }

        public ActionResult Image(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var image = from p in _db.Product_Image
                        select p;
            ViewBag.img = from p in _db.Product_Image
                          join pro in _db.Products on p.ID_Product equals pro.ID
                          select new mau
                          {
                              ID = p.ID,
                              ID_Product = (int)p.ID_Product,
                              Image = p.Image,
                              Name = pro.Name
                          };
            ViewBag.product = from b in _db.Products
                              select b;
            ViewBag.image = (from p in _db.Product_Image
                             join pro in _db.Products on p.ID_Product equals pro.ID
                             orderby p.ID descending
                             select new mau
                             {
                                 ID = p.ID,
                                 ID_Product = (int)p.ID_Product,
                                 Image = p.Image,
                                 Name = pro.Name
                             }).Take(3);
            return View(image.ToPagedList(page ?? 1, 5));
        }

        [HttpPost]
        public ActionResult Image(int? page, Product_Image productImage)
        {

            var image = from p in _db.Product_Image
                        select p;
            ViewBag.img = from p in _db.Product_Image
                          join pro in _db.Products on p.ID_Product equals pro.ID
                          select new mau
                          {
                              ID = p.ID,
                              ID_Product = (int)p.ID_Product,
                              Image = p.Image,
                              Name = pro.Name
                          };
            ViewBag.product = from b in _db.Products
                              select b;
            ViewBag.image = (from p in _db.Product_Image
                             join pro in _db.Products on p.ID_Product equals pro.ID
                             orderby p.ID descending
                             select new mau
                             {
                                 ID = p.ID,
                                 ID_Product = (int)p.ID_Product,
                                 Image = p.Image,
                                 Name = pro.Name
                             }).Take(3);
            var test = _db.Products.FirstOrDefault(p => p.ID == Convert.ToInt32(Request["ID_SP"]));
            ViewBag.date = DateTime.Now;
            if (test == null)
            {
                ViewBag.test = "Sản phẩm có ID " + Request["ID_SP"] + " không tồn tại!";
            }
            else
            {
                productImage.ID_Product = Convert.ToInt32(Request["ID_SP"]);
                productImage.Image = Request["Anh"];
                productImage.created_at = ViewBag.date;
                _db.Product_Image.Add(productImage);
                _db.SaveChanges();
                ViewBag.test = "Success!";
            }
            return View(image.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Delete_img(int id)
        {
            var img = _db.Product_Image.SingleOrDefault(b => b.ID == id);
            if (img != null) _db.Product_Image.Remove(img);
            _db.SaveChanges();
            return RedirectToAction("Image");
        }
    }
}