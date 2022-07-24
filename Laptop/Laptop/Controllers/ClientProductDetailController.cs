using Laptop.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Laptop.Controllers
{
    public class ClientProductDetailController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();
        // GET: _clientProductDetail

        public ActionResult Index()
        {

            var key = Convert.ToInt32(Request["key"]);
            Session["load"] = key;
            ViewBag.img = (from b in _db.Products
                           join proImg in _db.Product_Image on b.ID equals (proImg.ID_Product)
                           where b.ID == key
                           select proImg);
            ViewBag.ctsp = (from b in _db.Products
                            where b.ID == key
                            orderby b.ID descending
                            select b).Take(4);
            ViewBag.ID_pro = (from p in _db.Products
                              where p.ID == key
                              select new SoLuong
                              {
                                  ID_Product_Color = p.ID,
                              });
            ViewBag.color = from p in _db.Products
                            join proCo in _db.Product_Color on p.ID equals (proCo.ID_Product)
                            join co in _db.Colors on proCo.ID_Color equals (co.ID)
                            where p.ID == key
                            select new SoLuong
                            {
                                Quantity = (int)proCo.Quantity,
                                Color = co.Color,
                                ID_Product_Color = proCo.ID,
                                Image = co.Image
                            };
            ViewBag.count = (_db.Products
                .Join(_db.Product_Color, p => p.ID, proCo => (proCo.ID_Product), (p, proCo) => new { p, pro_co = proCo })
                .Join(_db.Colors, @t => @t.pro_co.ID_Color, co => (co.ID), (@t, co) => new { @t, co })
                .Where(@t => @t.@t.p.ID == key)
                .Select(@t =>
                    new SoLuong { Quantity = (int)@t.@t.pro_co.Quantity, Color = @t.co.Color, Image = @t.co.Image })).Count();

            ViewBag.comment = (from bd in _db.Bill_Detail
                               join b in _db.Bills on bd.ID_Bill equals b.ID
                               join cus in _db.Customers on b.ID_Customer equals cus.ID
                               join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                               join pro in _db.Products on proCo.ID_Product equals pro.ID
                               join bra in _db.Brands on pro.ID_Brand equals bra.ID
                               join co in _db.Colors on proCo.ID_Color equals co.ID
                               where pro.ID == key
                               orderby bd.ratetime
                               select new Bill_Detaill
                               {
                                   rate = bd.rate.ToString(),
                                   Pro_Color = co.Color,
                                   comment = bd.comment,
                                   Cus_Name = cus.Name,
                                   ratetime = bd.ratetime.ToString(),
                               });
            ViewBag.com_count = (from bd in _db.Bill_Detail
                                 join b in _db.Bills on bd.ID_Bill equals b.ID
                                 join cus in _db.Customers on b.ID_Customer equals cus.ID
                                 join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                                 join pro in _db.Products on proCo.ID_Product equals pro.ID
                                 join bra in _db.Brands on pro.ID_Brand equals bra.ID
                                 join co in _db.Colors on proCo.ID_Color equals co.ID
                                 where pro.ID == key && bd.ratetime != null
                                 orderby bd.ratetime descending
                                 select new Bill_Detaill
                                 {
                                     rate = bd.rate.ToString(),
                                     comment = bd.comment,
                                     Cus_Name = cus.Name,
                                     ratetime = bd.ratetime.ToString(),
                                 }).Count();
            ViewBag.count1s = (from bd in _db.Bill_Detail
                               join b in _db.Bills on bd.ID_Bill equals b.ID
                               join cus in _db.Customers on b.ID_Customer equals cus.ID
                               join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                               join pro in _db.Products on proCo.ID_Product equals pro.ID
                               join bra in _db.Brands on pro.ID_Brand equals bra.ID
                               join co in _db.Colors on proCo.ID_Color equals co.ID
                               where pro.ID == key && bd.rate == 1
                               orderby bd.ratetime
                               select new Bill_Detaill
                               {
                                   rate = bd.rate.ToString(),
                                   comment = bd.comment,
                                   Cus_Name = cus.Name,
                                   ratetime = bd.ratetime.ToString(),
                               }).Count();
            ViewBag.count2s = (from bd in _db.Bill_Detail
                               join b in _db.Bills on bd.ID_Bill equals b.ID
                               join cus in _db.Customers on b.ID_Customer equals cus.ID
                               join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                               join pro in _db.Products on proCo.ID_Product equals pro.ID
                               join bra in _db.Brands on pro.ID_Brand equals bra.ID
                               join co in _db.Colors on proCo.ID_Color equals co.ID
                               where pro.ID == key && bd.rate == 2
                               orderby bd.ratetime
                               select new Bill_Detaill
                               {
                                   rate = bd.rate.ToString(),
                                   comment = bd.comment,
                                   Cus_Name = cus.Name,
                                   ratetime = bd.ratetime.ToString(),
                               }).Count();
            ViewBag.count3s = (from bd in _db.Bill_Detail
                               join b in _db.Bills on bd.ID_Bill equals b.ID
                               join cus in _db.Customers on b.ID_Customer equals cus.ID
                               join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                               join pro in _db.Products on proCo.ID_Product equals pro.ID
                               join bra in _db.Brands on pro.ID_Brand equals bra.ID
                               join co in _db.Colors on proCo.ID_Color equals co.ID
                               where pro.ID == key && bd.rate == 3
                               orderby bd.ratetime
                               select new Bill_Detaill
                               {
                                   rate = bd.rate.ToString(),
                                   comment = bd.comment,
                                   Cus_Name = cus.Name,
                                   ratetime = bd.ratetime.ToString(),
                               }).Count();
            ViewBag.count4s = (from bd in _db.Bill_Detail
                               join b in _db.Bills on bd.ID_Bill equals b.ID
                               join cus in _db.Customers on b.ID_Customer equals cus.ID
                               join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                               join pro in _db.Products on proCo.ID_Product equals pro.ID
                               join bra in _db.Brands on pro.ID_Brand equals bra.ID
                               join co in _db.Colors on proCo.ID_Color equals co.ID
                               where pro.ID == key && bd.rate == 4
                               orderby bd.ratetime
                               select new Bill_Detaill
                               {
                                   rate = bd.rate.ToString(),
                                   comment = bd.comment,
                                   Cus_Name = cus.Name,
                                   ratetime = bd.ratetime.ToString(),
                               }).Count();
            ViewBag.count5s = (from bd in _db.Bill_Detail
                               join b in _db.Bills on bd.ID_Bill equals b.ID
                               join cus in _db.Customers on b.ID_Customer equals cus.ID
                               join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                               join pro in _db.Products on proCo.ID_Product equals pro.ID
                               join bra in _db.Brands on pro.ID_Brand equals bra.ID
                               join co in _db.Colors on proCo.ID_Color equals co.ID
                               where pro.ID == key && bd.rate == 5
                               orderby bd.ratetime
                               select new Bill_Detaill
                               {
                                   rate = bd.rate.ToString(),
                                   comment = bd.comment,
                                   Cus_Name = cus.Name,
                                   ratetime = bd.ratetime.ToString(),
                               }).Count();
            return View();
        }

        public ActionResult Product(int? page, string sortOrder)
        {
            if (Session["key"] == null)
            {
                var product = (from b in _db.Products
                               orderby b.ID descending
                               select b).ToList();
                return View(product.ToPagedList(page ?? 1, 12));
            }
            else
            {
                var key = Convert.ToString(Session["key"]);
                ViewBag.TK = "Có ";
                ViewBag.TK1 = " sản phẩm trùng với: " + key;
                var product = (from b in _db.Products
                               where b.Name.Contains(key)
                               select b).ToList();
                ViewBag.count = (from b in _db.Products
                                 where b.Name.Contains(key)
                                 select b).Count();
                switch (sortOrder)
                {
                    case "md":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key)
                                   orderby b.ID descending
                                   select b).ToList();
                        break;
                    case "15tr":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price < 15000000
                                   select b).ToList();
                        break;
                    case "15-20tr":

                        product = (from b in _db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                                   select b).ToList();
                        break;
                    case "20-25tr":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                                   select b).ToList();
                        break;
                    case "25-30tr":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                                   select b).ToList();
                        break;
                    case "30tr":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price > 30000000
                                   select b).ToList();
                        break;
                    case "I5":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key) && b.CPU.Contains("i5")
                                   select b).ToList();
                        break;
                    case "I7":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key) && b.CPU.Contains("i7")
                                   select b).ToList();
                        break;
                    case "tang":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key)
                                   orderby b.Promotion_Price
                                   select b).ToList();
                        break;
                    case "giam":
                        product = (from b in _db.Products
                                   where b.Name.Contains(key)
                                   orderby b.Promotion_Price descending
                                   select b).ToList();
                        break;
                }
                return View(product.ToPagedList(page ?? 1, 12));
            }
        }

        [HttpPost]
        public ActionResult Product(int? page, FormCollection data, string sortOrder)
        {
            var key = Request["key"];
            Session["key"] = Request["key"];
            ViewBag.TK = "Có ";
            ViewBag.TK1 = " sản phẩm trùng với: " + key;
            var product = (from b in _db.Products
                           where b.Name.Contains(key)
                           orderby b.ID descending
                           select b).ToList();
            ViewBag.count = (from b in _db.Products
                             where b.Name.Contains(key)
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                    product = (from b in _db.Products
                               where b.Name.Contains(key)
                               orderby b.ID descending
                               select b).ToList();
                    break;
                case "15tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price < 15000000
                               select b).ToList();
                    break;
                case "15-20tr":

                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                               select b).ToList();
                    break;
                case "20-25tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                               select b).ToList();
                    break;
                case "25-30tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                               select b).ToList();
                    break;
                case "30tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 30000000
                               select b).ToList();
                    break;
                case "I5":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i5")
                               select b).ToList();
                    break;
                case "I7":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i7")
                               select b).ToList();
                    break;
                case "tang":
                    product = (from b in _db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price
                               select b).ToList();
                    break;
                case "giam":
                    product = (from b in _db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price descending
                               select b).ToList();
                    break;
            }
            return View(product.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Productloctheogia(int? page, string sortOrder)
        {
            var key = Convert.ToString(Session["key"]);
            Session["sortOrdertk"] = Request["sortOrder"];
            var product = (from b in _db.Products
                           where b.Name.Contains(key)
                           orderby b.ID descending
                           select b).ToList();
            ViewBag.count = (from b in _db.Products
                             where b.Name.Contains(key)
                             select b).Count();

            switch (sortOrder)
            {
                case "md":
                    product = (from b in _db.Products
                               where b.Name.Contains(key)
                               orderby b.ID descending
                               select b).ToList();
                    break;
                case "15tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price < 15000000
                               select b).ToList();
                    break;
                case "15-20tr":

                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                               select b).ToList();
                    break;
                case "20-25tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                               select b).ToList();
                    break;
                case "25-30tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                               select b).ToList();
                    break;
                case "30tr":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 30000000
                               select b).ToList();
                    break;
                case "I5":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i5")
                               select b).ToList();
                    break;
                case "I7":
                    product = (from b in _db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i7")
                               select b).ToList();
                    break;
                case "tang":
                    product = (from b in _db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price
                               select b).ToList();
                    break;
                case "giam":
                    product = (from b in _db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price descending
                               select b).ToList();
                    break;
            }
            return View(product.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Brand(int? page, string sortOrder)
        {
            Session["Brand"] = Request["Brand"];
            var bran = Convert.ToString(Session["Brand"]);
            ViewBag.tb = "Sản phẩm hãng " + Session["Brand"];
            var brand = (from b in _db.Products
                         join br in _db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == bran
                         orderby b.ID descending
                         select b).ToList();
            ViewBag.count = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.ID descending
                             select b).ToList();
                    break;
                case "15tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;
                case "15-20tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i7")
                             select b).ToList();
                    break;

                case "tang":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
            }
            return View(brand.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Brandloctheogia(int? page, string sortOrder)
        {
            Session["sortOrderbr"] = Request["sortOrder"];
            var bran = Convert.ToString(Session["Brand"]);

            var brand = (from b in _db.Products
                         join br in _db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == bran
                         orderby b.ID descending
                         select b).ToList();
            ViewBag.count = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.ID descending
                             select b).ToList();
                    break;
                case "15tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;
                case "15-20tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i7")
                             select b).ToList();
                    break;

                case "tang":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
            }

            return View(brand.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Dell(int? page, string sortOrder)
        {
            ViewBag.tb = "Sản phẩm hãng " + "Dell";

            var brand = (from b in _db.Products
                         join br in _db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == "Dell"
                         select b).ToList();
            ViewBag.count = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             select b).Count();

            return View(brand.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Loctheogia(int? page, string sortOrder)
        {
            ViewBag.tb = "Sản phẩm hãng " + "Dell";

            var brand = (from b in _db.Products
                         join br in _db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == "Dell"
                         select b).ToList();
            ViewBag.count = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             select b).ToList();
                    break;
                case "15tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;

                case "15-20tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.CPU.Contains("i7")
                             select b).ToList();
                    break;
                case "tang":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
            }
            return View(brand.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Acer(int? page, string sortOrder)
        {
            string bran = "Acer";
            ViewBag.tb = "Sản phẩm hãng " + "Acer";

            var brand = (from b in _db.Products
                         join br in _db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == bran
                         select b).ToList();
            ViewBag.count = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).Count();

            return View(brand.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Acerloctheogia(int? page, string sortOrder)
        {
            ViewBag.tb = "Sản phẩm hãng " + "Acer";
            var bran = "Acer";
            var brand = (from b in _db.Products
                         join br in _db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == bran
                         select b).ToList();
            ViewBag.count = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).ToList();
                    break;
                case "15tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;
                case "15-20tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i7")
                             select b).ToList();
                    break;

                case "tang":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in _db.Products
                             join br in _db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
            }

            return View(brand.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Phanloai(int? page, string sortOrder)
        {

            Session["group"] = Request["group"];
            string grou = Request["group"];
            ViewBag.tb = " sản phẩm: " + grou + "!";
            var gro = (from p in _db.Products
                       where p.Group_Pro == grou
                       orderby p.ID descending
                       select p).ToList();
            ViewBag.count = (from p in _db.Products
                             where p.Group_Pro == grou
                             select p).Count();
            switch (sortOrder)
            {
                case "md":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou
                           orderby p.ID descending
                           select p).ToList();
                    break;
                case "15tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price < 15000000
                           select p).ToList();
                    break;
                case "15-20tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price >= 15000000 && p.Promotion_Price <= 20000000
                           select p).ToList();
                    break;
                case "20-25tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 20000000 && p.Promotion_Price <= 25000000
                           select p).ToList();
                    break;
                case "25-30tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 25000000 && p.Promotion_Price <= 30000000
                           select p).ToList();
                    break;
                case "30tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 30000000
                           select p).ToList();
                    break;
                case "I5":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i5")
                           select p).ToList();
                    break;
                case "I7":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i7")
                           select p).ToList();
                    break;

                case "tang":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price
                           select p).ToList();
                    break;
                case "giam":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price descending
                           select p).ToList();
                    break;
            }
            return View(gro.ToPagedList(page ?? 1, 12));
        }

        public ActionResult Phanloailoctheogia(int? page, string sortOrder)
        {
            Session["sortOrderpl"] = Request["sortOrder"];
            var grou = Convert.ToString(Session["group"]);

            var gro = (from p in _db.Products
                       where p.Group_Pro == grou
                       orderby p.ID descending
                       select p).ToList();
            ViewBag.count = (from p in _db.Products
                             where p.Group_Pro == grou
                             select p).Count();
            switch (sortOrder)
            {
                case "md":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou
                           orderby p.ID descending
                           select p).ToList();
                    break;
                case "15tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price < 15000000
                           select p).ToList();
                    break;
                case "15-20tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price >= 15000000 && p.Promotion_Price <= 20000000
                           select p).ToList();
                    break;
                case "20-25tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 20000000 && p.Promotion_Price <= 25000000
                           select p).ToList();
                    break;
                case "25-30tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 25000000 && p.Promotion_Price <= 30000000
                           select p).ToList();
                    break;
                case "30tr":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 30000000
                           select p).ToList();
                    break;
                case "I5":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i5")
                           select p).ToList();
                    break;
                case "I7":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i7")
                           select p).ToList();
                    break;

                case "tang":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price
                           select p).ToList();
                    break;
                case "giam":
                    gro = (from p in _db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price descending
                           select p).ToList();
                    break;
            }
            return View(gro.ToPagedList(page ?? 1, 12));
        }
    }
}