using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using PagedList;
namespace Laptop.Controllers
{
    public class _clientProductDetailController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: _clientProductDetail
       
        public ActionResult Index()
        {

            int key = Convert.ToInt32( Request["key"]);
            Session["load"] = key;
            ViewBag.img = (from b in db.Products
                            join pro_img in db.Product_Images on b.ID equals (pro_img.ID_Product)
                            where b.ID == key
                            select pro_img);
            ViewBag.ctsp = (from b in db.Products
                            where b.ID == key
                            orderby b.ID descending
                            select b).Take(4);
            ViewBag.ID_pro = (from p in db.Products
                             where p.ID == key
                             select new SoLuong
                             {
                                 ID_Product_Color = (int)p.ID,
                             }); 
            ViewBag.color = from p in db.Products
                            join pro_co in db.Product_Colors on p.ID equals (pro_co.ID_Product)
                            join co in db.Colorrs on pro_co.ID_Color equals (co.ID)
                            where p.ID == key
                            select new SoLuong
                            {
                                Quantity = (int)pro_co.Quantity,
                                Color = co.Color,
                                ID_Product_Color = (int)pro_co.ID, 
                                Image = co.Image
                            };
            ViewBag.count = (from p in db.Products
                            join pro_co in db.Product_Colors on p.ID equals (pro_co.ID_Product)
                            join co in db.Colorrs on pro_co.ID_Color equals (co.ID)
                            where p.ID == key
                            select new SoLuong
                            {
                                Quantity = (int)pro_co.Quantity,
                                Color = co.Color,
                                Image = co.Image
                            }).Count();
           
            ViewBag.comment = (from bd in db.Bill_Details
                               join b in db.Bills on bd.ID_Bill equals b.ID
                               join cus in db.Customers on b.ID_Customer equals cus.ID
                               join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                               join pro in db.Products on pro_co.ID_Product equals pro.ID
                               join bra in db.Brands on pro.ID_Brand equals bra.ID
                               join co in db.Colorrs on pro_co.ID_Color equals co.ID
                               where pro.ID == key
                               orderby bd.ratetime
                               select new Bill_Detaill
                               {
                                   rate = bd.rate.ToString(),
                                   Pro_Color= co.Color,
                                   comment = bd.comment,
                                   Cus_Name = cus.Name,
                                   ratetime = bd.ratetime.ToString(),
                               });
            ViewBag.com_count = (from bd in db.Bill_Details
                                 join b in db.Bills on bd.ID_Bill equals b.ID
                                 join cus in db.Customers on b.ID_Customer equals cus.ID
                                 join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                 join pro in db.Products on pro_co.ID_Product equals pro.ID
                                 join bra in db.Brands on pro.ID_Brand equals bra.ID
                                 join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                 where pro.ID == key && bd.ratetime != null
                                 orderby bd.ratetime descending
                                 select new Bill_Detaill
                                 {
                                     rate = bd.rate.ToString(),
                                     comment = bd.comment,
                                     Cus_Name = cus.Name,
                                     ratetime = bd.ratetime.ToString(),
                                 }).Count();
            ViewBag.count1s = (from bd in db.Bill_Details
                                 join b in db.Bills on bd.ID_Bill equals b.ID
                                 join cus in db.Customers on b.ID_Customer equals cus.ID
                                 join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                 join pro in db.Products on pro_co.ID_Product equals pro.ID
                                 join bra in db.Brands on pro.ID_Brand equals bra.ID
                                 join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                 where pro.ID == key && bd.rate == 1
                                 orderby bd.ratetime 
                                 select new Bill_Detaill
                                 {
                                     rate = bd.rate.ToString(),
                                     comment = bd.comment,
                                     Cus_Name = cus.Name,
                                     ratetime = bd.ratetime.ToString(),
                                 }).Count();
            ViewBag.count2s = (from bd in db.Bill_Details
                                 join b in db.Bills on bd.ID_Bill equals b.ID
                                 join cus in db.Customers on b.ID_Customer equals cus.ID
                                 join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                 join pro in db.Products on pro_co.ID_Product equals pro.ID
                                 join bra in db.Brands on pro.ID_Brand equals bra.ID
                                 join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                 where pro.ID == key && bd.rate == 2
                                 orderby bd.ratetime 
                                 select new Bill_Detaill
                                 {
                                     rate = bd.rate.ToString(),
                                     comment = bd.comment,
                                     Cus_Name = cus.Name,
                                     ratetime = bd.ratetime.ToString(),
                                 }).Count();
            ViewBag.count3s = (from bd in db.Bill_Details
                                 join b in db.Bills on bd.ID_Bill equals b.ID
                                 join cus in db.Customers on b.ID_Customer equals cus.ID
                                 join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                 join pro in db.Products on pro_co.ID_Product equals pro.ID
                                 join bra in db.Brands on pro.ID_Brand equals bra.ID
                                 join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                 where pro.ID == key && bd.rate == 3
                                 orderby bd.ratetime 
                                 select new Bill_Detaill
                                 {
                                     rate = bd.rate.ToString(),
                                     comment = bd.comment,
                                     Cus_Name = cus.Name,
                                     ratetime = bd.ratetime.ToString(),
                                 }).Count();
            ViewBag.count4s = (from bd in db.Bill_Details
                                 join b in db.Bills on bd.ID_Bill equals b.ID
                                 join cus in db.Customers on b.ID_Customer equals cus.ID
                                 join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                 join pro in db.Products on pro_co.ID_Product equals pro.ID
                                 join bra in db.Brands on pro.ID_Brand equals bra.ID
                                 join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                 where pro.ID == key && bd.rate == 4
                                 orderby bd.ratetime 
                                 select new Bill_Detaill
                                 {
                                     rate = bd.rate.ToString(),
                                     comment = bd.comment,
                                     Cus_Name = cus.Name,
                                     ratetime = bd.ratetime.ToString(),
                                 }).Count();
            ViewBag.count5s = (from bd in db.Bill_Details
                                 join b in db.Bills on bd.ID_Bill equals b.ID
                                 join cus in db.Customers on b.ID_Customer equals cus.ID
                                 join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                 join pro in db.Products on pro_co.ID_Product equals pro.ID
                                 join bra in db.Brands on pro.ID_Brand equals bra.ID
                                 join co in db.Colorrs on pro_co.ID_Color equals co.ID
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
        public ActionResult product(int? page, string sortOrder)
        {
            if (Session["key"] == null)
            {
                var product = (from b in db.Products
                               orderby b.ID descending
                               select b).ToList();
                return View(product.ToPagedList(page ?? 1, 12));
            }
            else
            {
                string key =Convert.ToString(Session["key"]);
                ViewBag.TK = "Có ";
                ViewBag.TK1 = " sản phẩm trùng với: " + key;
                var product = (from b in db.Products
                               where b.Name.Contains(key)
                               select b).ToList();
                ViewBag.count = (from b in db.Products
                                 where b.Name.Contains(key)
                                 select b).Count();
                switch (sortOrder)
                {
                    case "md":
                        product = (from b in db.Products
                                   where b.Name.Contains(key)
                                   orderby b.ID descending
                                   select b).ToList();
                        break;
                    case "15tr":
                        product = (from b in db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price < 15000000
                                   select b).ToList();
                        break;
                    case "15-20tr":

                        product = (from b in db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                                   select b).ToList();
                        break;
                    case "20-25tr":
                        product = (from b in db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                                   select b).ToList();
                        break;
                    case "25-30tr":
                        product = (from b in db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                                   select b).ToList();
                        break;
                    case "30tr":
                        product = (from b in db.Products
                                   where b.Name.Contains(key) && b.Promotion_Price > 30000000
                                   select b).ToList();
                        break;
                    case "I5":
                        product = (from b in db.Products
                                   where b.Name.Contains(key) && b.CPU.Contains("i5")
                                   select b).ToList();
                        break;
                    case "I7":
                        product = (from b in db.Products
                                   where b.Name.Contains(key) && b.CPU.Contains("i7")
                                   select b).ToList();
                        break;
                    case "tang":
                        product = (from b in db.Products
                                   where b.Name.Contains(key)
                                   orderby b.Promotion_Price
                                   select b).ToList();
                        break;
                    case "giam":
                        product = (from b in db.Products
                                   where b.Name.Contains(key)
                                   orderby b.Promotion_Price descending
                                   select b).ToList();
                        break;
                    default:
                        break;
                }
                return View(product.ToPagedList(page ?? 1, 12));
            }
        }
        [HttpPost]
        public ActionResult product(int? page,FormCollection data, string sortOrder)
        {
            
            string key = Request["key"];
            Session["key"] = Request["key"];
            ViewBag.TK = "Có ";
            ViewBag.TK1=" sản phẩm trùng với: " + key;
            var product = (from b in db.Products
                          where b.Name.Contains(key)
                           orderby b.ID descending
                           select b).ToList();
            ViewBag.count = (from b in db.Products
                          where b.Name.Contains(key)
                          select b).Count();
            switch (sortOrder)
            {
                case "md":
                    product = (from b in db.Products
                               where b.Name.Contains(key)
                               orderby b.ID descending
                               select b).ToList();
                    break;
                case "15tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price < 15000000
                               select b).ToList();
                    break;
                case "15-20tr":

                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                               select b).ToList();
                    break;
                case "20-25tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                               select b).ToList();
                    break;
                case "25-30tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                               select b).ToList();
                    break;
                case "30tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 30000000
                               select b).ToList();
                    break;
                case "I5":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i5")
                               select b).ToList();
                    break;
                case "I7":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i7")
                               select b).ToList();
                    break;
                case "tang":
                    product = (from b in db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price
                               select b).ToList();
                    break;
                case "giam":
                    product = (from b in db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price descending
                               select b).ToList();
                    break;
                default:
                    break;
            }
            return View(product.ToPagedList(page ?? 1, 12));
        }
        
        public ActionResult productloctheogia(int? page, string sortOrder)
        {
            string key = Convert.ToString(Session["key"]);
            Session["sortOrdertk"] = Request["sortOrder"];
            var product = (from b in db.Products
                           where b.Name.Contains(key)
                           orderby b.ID descending
                           select b).ToList();
            ViewBag.count = (from b in db.Products
                             where b.Name.Contains(key)
                             select b).Count();

            switch (sortOrder)
            {
                case "md":
                    product = (from b in db.Products
                               where b.Name.Contains(key)
                               orderby b.ID descending
                               select b).ToList();
                    break;
                case "15tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price < 15000000
                               select b).ToList();
                    break;
                case "15-20tr":

                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                               select b).ToList();
                    break;
                case "20-25tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                               select b).ToList();
                    break;
                case "25-30tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                               select b).ToList();
                    break;
                case "30tr":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.Promotion_Price > 30000000
                               select b).ToList();
                    break;
                case "I5":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i5")
                               select b).ToList();
                    break;
                case "I7":
                    product = (from b in db.Products
                               where b.Name.Contains(key) && b.CPU.Contains("i7")
                               select b).ToList();
                    break;
                case "tang":
                    product = (from b in db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    product = (from b in db.Products
                               where b.Name.Contains(key)
                               orderby b.Promotion_Price descending
                               select b).ToList();
                    break;
                default:
                    break;
            }
            return View(product.ToPagedList(page ?? 1, 12));
        }
        public ActionResult brand(int? page, string sortOrder)
        {                     
            Session["brand"] = Request["brand"];
            string bran = Convert.ToString(Session["brand"]);
            ViewBag.tb = "Sản phẩm hãng " + Session["brand"]; 
            var brand = (from b in db.Products
                            join br in db.Brands on b.ID_Brand equals (br.ID)
                            where br.Name == bran
                         orderby b.ID descending
                         select b).ToList();
            ViewBag.count= (from b in db.Products
                            join br in db.Brands on b.ID_Brand equals (br.ID)
                            where br.Name == bran
                            select b).Count();
            switch (sortOrder)
            {
                case "md":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.ID descending
                             select b).ToList();
                    break;
                case "15tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;
                case "15-20tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i7")
                             select b).ToList();
                    break;

                case "tang":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
                default:
                    break;
            }
            return View(brand.ToPagedList(page ?? 1, 12));
        }
        public ActionResult brandloctheogia(int? page, string sortOrder )
        {
            Session["sortOrderbr"] = Request["sortOrder"];
            string bran = Convert.ToString(Session["brand"]);

            var brand = (from b in db.Products
                         join br in db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == bran
                         orderby b.ID descending
                         select b).ToList();
            ViewBag.count = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.ID descending
                             select b).ToList();
                    break;
                case "15tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;
                case "15-20tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i7")
                             select b).ToList();
                    break;

                case "tang":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
                default:
                    break;
            }

            return View(brand.ToPagedList(page ?? 1, 12));
        }
        public ActionResult Dell(int? page, string sortOrder)
        {
            ViewBag.tb = "Sản phẩm hãng " + "Dell";
            
            var brand = (from b in db.Products
                            join br in db.Brands on b.ID_Brand equals (br.ID)
                            where br.Name == "Dell"
                            select b).ToList();
            ViewBag.count= (from b in db.Products
                            join br in db.Brands on b.ID_Brand equals (br.ID)
                            where br.Name == "Dell"
                            select b).Count();

            return View(brand.ToPagedList(page ?? 1, 12));
        }
        public ActionResult loctheogia(int? page, string sortOrder)
        {
            ViewBag.tb = "Sản phẩm hãng " + "Dell";

            var brand = (from b in db.Products
                         join br in db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == "Dell"
                         select b).ToList();
            ViewBag.count = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                     brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" 
                             select b).ToList();
                    break;
                case "15tr":
                     brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;

                case "15-20tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell" && b.CPU.Contains("i7")
                             select b).ToList();
                    break;
                case "tang":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == "Dell"
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
                default:
                    break;
            }

            return View(brand.ToPagedList(page ?? 1, 12));
        }
        public ActionResult Acer(int? page, string sortOrder)
        {
            string bran = "Acer";
            ViewBag.tb = "Sản phẩm hãng " + "Acer";

            var brand = (from b in db.Products
                         join br in db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == bran
                         select b).ToList();
            ViewBag.count = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).Count();            

            return View(brand.ToPagedList(page ?? 1, 12));
        }
        public ActionResult acerloctheogia(int? page, string sortOrder)
        {
            ViewBag.tb = "Sản phẩm hãng " + "Acer";
            string bran = "Acer";
            var brand = (from b in db.Products
                         join br in db.Brands on b.ID_Brand equals (br.ID)
                         where br.Name == bran
                         select b).ToList();
            ViewBag.count = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).Count();
            switch (sortOrder)
            {
                case "md":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             select b).ToList();
                    break;
                case "15tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price < 15000000
                             select b).ToList();
                    break;
                case "15-20tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price >= 15000000 && b.Promotion_Price <= 20000000
                             select b).ToList();
                    break;
                case "20-25tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 20000000 && b.Promotion_Price <= 25000000
                             select b).ToList();
                    break;
                case "25-30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 25000000 && b.Promotion_Price <= 30000000
                             select b).ToList();
                    break;
                case "30tr":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.Promotion_Price > 30000000
                             select b).ToList();
                    break;
                case "I5":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i5")
                             select b).ToList();
                    break;
                case "I7":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran && b.CPU.Contains("i7")
                             select b).ToList();
                    break;

                case "tang":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price
                             select b).ToList();
                    break;
                case "giam":
                    brand = (from b in db.Products
                             join br in db.Brands on b.ID_Brand equals (br.ID)
                             where br.Name == bran
                             orderby b.Promotion_Price descending
                             select b).ToList();
                    break;
                default:
                    break;
            }

            return View(brand.ToPagedList(page ?? 1, 12));
        }
        public ActionResult phanloai(int? page, string sortOrder)
        {
            
            Session["group"] = Request["group"];
            string grou = Request["group"];
            ViewBag.tb = " sản phẩm: " + grou + "!";
            var gro = (from p in db.Products
                      where p.Group_Pro == grou
                       orderby p.ID descending
                       select p).ToList();
            ViewBag.count = (from p in db.Products
                      where p.Group_Pro == grou
                      select p).Count();
            switch (sortOrder)
            {
                case "md":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou
                           orderby p.ID descending
                           select p).ToList();
                    break;
                case "15tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price < 15000000
                           select p).ToList();
                    break;
                case "15-20tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price >= 15000000 && p.Promotion_Price <= 20000000
                           select p).ToList();
                    break;
                case "20-25tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 20000000 && p.Promotion_Price <= 25000000
                           select p).ToList();
                    break;
                case "25-30tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 25000000 && p.Promotion_Price <= 30000000
                           select p).ToList();
                    break;
                case "30tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 30000000
                           select p).ToList();
                    break;
                case "I5":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i5")
                           select p).ToList();
                    break;
                case "I7":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i7")
                           select p).ToList();
                    break;

                case "tang":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price
                           select p).ToList();
                    break;
                case "giam":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price descending
                           select p).ToList();
                    break;
                default:
                    break;
            }
            return View(gro.ToPagedList(page ?? 1, 12));
        }
        public ActionResult phanloailoctheogia(int? page, string sortOrder)
        {
            Session["sortOrderpl"] = Request["sortOrder"];
            string grou = Convert.ToString(Session["group"]);

            var gro = (from p in db.Products
                       where p.Group_Pro == grou
                       orderby p.ID descending
                       select p).ToList();
            ViewBag.count = (from p in db.Products
                             where p.Group_Pro == grou
                             select p).Count();
            switch (sortOrder)
            {
                case "md":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou
                           orderby p.ID descending
                           select p).ToList();
                    break;
                case "15tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price < 15000000
                             select p).ToList();
                    break;
                case "15-20tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price >= 15000000 && p.Promotion_Price <= 20000000
                             select p).ToList();
                    break;
                case "20-25tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 20000000 && p.Promotion_Price <= 25000000
                             select p).ToList();
                    break;
                case "25-30tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 25000000 && p.Promotion_Price <= 30000000
                             select p).ToList();
                    break;
                case "30tr":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.Promotion_Price > 30000000
                             select p).ToList();
                    break;
                case "I5":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i5")
                             select p).ToList();
                    break;
                case "I7":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou && p.CPU.Contains("i7")
                             select p).ToList();
                    break;

                case "tang":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price
                             select p).ToList();
                    break;
                case "giam":
                    gro = (from p in db.Products
                           where p.Group_Pro == grou
                           orderby p.Promotion_Price descending
                             select p).ToList();
                    break;
                default:
                    break;
            }

            return View(gro.ToPagedList(page ?? 1, 12));
        }
    }
}