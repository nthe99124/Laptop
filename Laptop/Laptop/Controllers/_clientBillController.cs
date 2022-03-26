using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using Laptop.Controllers;
using System.Data;
using System.Data.Entity;

namespace Laptop.Controllers
{
    public class _clientBillController : Controller
    {
        LaptopNTT db = new LaptopNTT();
        // GET: _clientBill
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Buy()
        {
            var IdCus = Convert.ToInt32(Session["ID_cus"]);
            int key = Convert.ToInt32(Request["key"]);
            Session["key"] = Convert.ToInt32(Request["key"]);
            ViewBag.cus = from cus in db.Customers
                          where cus.ID == IdCus
                          select cus;
            ViewBag.color = from pro in db.Products
                            join pr_co in db.Product_Color on pro.ID equals pr_co.ID_Product
                            join co in db.Colorrs on pr_co.ID_Color equals co.ID
                            where pro.ID == key
                            select new client_bill
                            {
                                Pro_Color = co.Color,
                                ID_Pro_Co = (int)pr_co.ID
                            };
            ViewBag.pro = (from pro in db.Products
                           join pr_co in db.Product_Color on pro.ID equals pr_co.ID_Product
                           join br in db.Brands on pro.ID_Brand equals br.ID
                           join co in db.Colorrs on pr_co.ID_Color equals co.ID
                           where pro.ID == key
                           select new client_bill
                           {
                               Pro_ID = (int)pro.ID,
                               Pro_Image = pro.Image,
                               Pro_Name = pro.Name,
                               Pro_Brand = br.Name,
                               Pro_Price = (int)pro.Promotion_Price

                           }).Take(1);

            return View();
        }
        public ActionResult pay()
        {
            return View();
        }
        [HttpPost]
        public ActionResult pay(Product pro, int id)
        {
            var ProColor = Convert.ToInt32(Request["Pro_Color"]);
            pro = db.Products.Where(m => m.ID == id).SingleOrDefault();
            ViewBag.color = from pr in db.Products
                            join pr_co in db.Product_Color on pr.ID equals pr_co.ID_Product
                            join co in db.Colorrs on pr_co.ID_Color equals co.ID
                            where pr_co.ID == ProColor
                            select new client_bill
                            {
                                Pro_Color = co.Color,
                            };
            Session["sdt"] = Request["phone"];
            Session["add"] = Request["add"];
            Session["image"] = pro.Image;
            Session["pro_name"] = pro.Name;
            Session["color"] = Request["Pro_Color"];
            Session["quan"] = Request["quan"];
            Session["price"] = pro.Promotion_Price;
            return View();
        }
        public ActionResult Bill()
        {
            Session["color"] = null;
            Bill bill = db.Bills.OrderByDescending(m => m.Date_order).Take(1).SingleOrDefault();
            ViewBag.bill_detail = (from bd in db.Bill_Detail
                                   join b in db.Bills on bd.ID_Bill equals b.ID
                                   join cus in db.Customers on b.ID_Customer equals cus.ID
                                   join pro_co in db.Product_Color on bd.ID_Product_Color equals pro_co.ID
                                   join pro in db.Products on pro_co.ID_Product equals pro.ID
                                   join bra in db.Brands on pro.ID_Brand equals bra.ID
                                   join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                   where b.ID == bill.ID
                                   select new Bill_Detaill
                                   {
                                       ID = bd.ID,
                                       ID_Bill = b.ID,
                                       Quantity = (int)bd.Quantity,
                                       Cus_Name = cus.Name,
                                       Cus_Email = cus.Email,
                                       Bill_Add = b.Address,
                                       Cus_Phone = b.Phone_Number,
                                       Cus_Gender = cus.Gender,
                                       Pro_Name = pro.Name,
                                       Pro_Brand = bra.Name,
                                       Order_Price = (decimal)bd.order_price,
                                       Pro_Color = co.Color
                                   }).Distinct();

            ViewBag.Cus_Detail = (from bd in db.Bill_Detail
                                  join b in db.Bills on bd.ID_Bill equals b.ID
                                  join cus in db.Customers on b.ID_Customer equals cus.ID
                                  where bd.ID_Bill == bill.ID
                                  select new Bill_Detaill
                                  {
                                      ID_Bill = b.ID,
                                      Cus_Name = cus.Name,
                                      Cus_Email = cus.Email,
                                      Cus_Phone = b.Phone_Number,
                                      Bill_Add = b.Address
                                  }).Distinct();
            return View();
        }
        [HttpPost]
        public ActionResult Bill(Bill b, Bill_Detail bi)
        {
            if (Session["color"] != null)
            {
                b.Date_order = DateTime.Now;
                b.ID_Customer = Convert.ToInt32(Session["ID_cus"]);
                b.Address = Convert.ToString(Session["add"]);
                b.Phone_Number = Convert.ToString(Session["sdt"]);
                b.Confirm = "Chờ xác nhận";
                db.Bills.Add(b);
                db.SaveChanges();
                Bill bill = db.Bills.OrderByDescending(m => m.Date_order).Take(1).SingleOrDefault();

                bi.ID_Bill = bill.ID;
                bi.ID_Product_Color = Convert.ToInt32(Session["color"]);
                bi.Quantity = Convert.ToInt32(Session["quan"]);
                bi.order_price = Convert.ToInt32(Session["price"]);
                db.Bill_Detail.Add(bi);
                db.SaveChanges();
                return this.Bill();
            }
            return View();
        }
        public ActionResult Bill_cus()
        {
            var bill = from b in db.Bills
                       where b.ID_Customer == Convert.ToInt32(Session["ID_cus"])
                       orderby b.Date_order descending
                       select b;
            var IDcus = Convert.ToInt32(Session["ID_cus"]);
            ViewBag.name = (from b in db.Bills
                            join cus in db.Customers on b.ID_Customer equals cus.ID
                            where b.ID_Customer == IDcus
                            orderby b.Date_order descending
                            select new Bill_Detaill
                            {
                                Cus_Name = cus.Name
                            }).Distinct();
            return View(bill);
        }
        public ActionResult Bill_Detail(int id)
        {
            ViewBag.bill_detail = (from bd in db.Bill_Detail
                                   join b in db.Bills on bd.ID_Bill equals b.ID
                                   join cus in db.Customers on b.ID_Customer equals cus.ID
                                   join pro_co in db.Product_Color on bd.ID_Product_Color equals pro_co.ID
                                   join pro in db.Products on pro_co.ID_Product equals pro.ID
                                   join bra in db.Brands on pro.ID_Brand equals bra.ID
                                   join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                   where bd.ID_Bill == id
                                   select new Bill_Detaill
                                   {
                                       ID = bd.ID,
                                       ID_Bill = b.ID,
                                       Quantity = (int)bd.Quantity,
                                       Cus_Name = cus.Name,
                                       Cus_Email = cus.Email,
                                       Bill_Add = b.Address,
                                       Cus_Phone = b.Phone_Number,
                                       Cus_Gender = cus.Gender,
                                       Pro_Name = pro.Name,
                                       Pro_Brand = bra.Name,
                                       Order_Price = (decimal)bd.order_price,
                                       Pro_Color = co.Color
                                   }).Distinct();
            ViewBag.Cus_Detail = (from bd in db.Bill_Detail
                                  join b in db.Bills on bd.ID_Bill equals b.ID
                                  join cus in db.Customers on b.ID_Customer equals cus.ID
                                  where bd.ID_Bill == id
                                  select new Bill_Detaill
                                  {
                                      ID_Bill = b.ID,
                                      Cus_Name = cus.Name,
                                      Cus_Email = cus.Email,
                                      Cus_Phone = b.Phone_Number,
                                      Bill_Add = b.Address,
                                      date = Convert.ToDateTime(b.Date_order)
                                  }).Distinct();
            return View();
        }
        public ActionResult Confirm(Bill bill)
        {
            int id = Convert.ToInt32(Request["id"]);
            string conf = (Request["confirm"]);
            bill = db.Bills.Where(b => b.ID == id).SingleOrDefault();
            bill.Confirm = conf;
            db.Entry(bill).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Bill_cus");
        }
        public ActionResult rate()
        {
            int id = Convert.ToInt32(Request["id"]);
            Session["id_bd"] = id;
            ViewBag.bill_detail = (from bd in db.Bill_Detail
                                   join b in db.Bills on bd.ID_Bill equals b.ID
                                   join cus in db.Customers on b.ID_Customer equals cus.ID
                                   join pro_co in db.Product_Color on bd.ID_Product_Color equals pro_co.ID
                                   join pro in db.Products on pro_co.ID_Product equals pro.ID
                                   join bra in db.Brands on pro.ID_Brand equals bra.ID
                                   join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                   where bd.ID_Bill == id
                                   select new Bill_Detaill
                                   {
                                       ID = bd.ID,
                                       ID_Bill = b.ID,
                                       ID_Bill_Detail = bd.ID,
                                       Quantity = (int)bd.Quantity,
                                       Cus_Name = cus.Name,
                                       Cus_Email = cus.Email,
                                       Bill_Add = b.Address,
                                       Cus_Phone = b.Phone_Number,
                                       Cus_Gender = cus.Gender,
                                       Pro_Name = pro.Name,
                                       Pro_Brand = bra.Name,
                                       Order_Price = (decimal)bd.order_price,
                                       Pro_Color = co.Color,
                                       rate = bd.rate.ToString(),
                                       comment = bd.comment
                                   }).Distinct();
            ViewBag.Cus_Detail = (from bd in db.Bill_Detail
                                  join b in db.Bills on bd.ID_Bill equals b.ID
                                  join cus in db.Customers on b.ID_Customer equals cus.ID
                                  where bd.ID_Bill == id
                                  select new Bill_Detaill
                                  {
                                      ID_Bill = b.ID,
                                      Cus_Name = cus.Name,
                                      Cus_Email = cus.Email,
                                      Cus_Phone = b.Phone_Number,
                                      Bill_Add = b.Address,
                                      date = Convert.ToDateTime(b.Date_order)
                                  }).Distinct();
            return View();
        }
        [HttpPost]
        public ActionResult rate(Bill_Detail bill_de)
        {
            int key = Convert.ToInt32(Request["key"]);
            int id = Convert.ToInt32(Session["id_bd"]);
            ViewBag.bill_detail = (from bd in db.Bill_Detail
                                   join b in db.Bills on bd.ID_Bill equals b.ID
                                   join cus in db.Customers on b.ID_Customer equals cus.ID
                                   join pro_co in db.Product_Color on bd.ID_Product_Color equals pro_co.ID
                                   join pro in db.Products on pro_co.ID_Product equals pro.ID
                                   join bra in db.Brands on pro.ID_Brand equals bra.ID
                                   join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                   where bd.ID_Bill == id
                                   select new Bill_Detaill
                                   {
                                       ID = bd.ID,
                                       ID_Bill = b.ID,
                                       ID_Bill_Detail = bd.ID,
                                       Quantity = (int)bd.Quantity,
                                       Cus_Name = cus.Name,
                                       Cus_Email = cus.Email,
                                       Bill_Add = b.Address,
                                       Cus_Phone = b.Phone_Number,
                                       Cus_Gender = cus.Gender,
                                       Pro_Name = pro.Name,
                                       Pro_Brand = bra.Name,
                                       Order_Price = (decimal)bd.order_price,
                                       Pro_Color = co.Color,
                                       rate = bd.rate.ToString(),
                                       comment = bd.comment
                                   }).Distinct();
            ViewBag.Cus_Detail = (from bd in db.Bill_Detail
                                  join b in db.Bills on bd.ID_Bill equals b.ID
                                  join cus in db.Customers on b.ID_Customer equals cus.ID
                                  where bd.ID_Bill == id
                                  select new Bill_Detaill
                                  {
                                      ID_Bill = b.ID,
                                      Cus_Name = cus.Name,
                                      Cus_Email = cus.Email,
                                      Cus_Phone = b.Phone_Number,
                                      Bill_Add = b.Address,
                                      date = Convert.ToDateTime(b.Date_order)
                                  }).Distinct();

            bill_de = db.Bill_Detail.Where(a => a.ID == key).SingleOrDefault();
            bill_de.comment = (Request["comment"]);
            bill_de.ratetime = DateTime.Now;

            bill_de.rate = Convert.ToInt32(Request["star"]);

            db.Entry(bill_de).State = EntityState.Modified;
            db.SaveChanges();
            return this.View();
        }

    }
}