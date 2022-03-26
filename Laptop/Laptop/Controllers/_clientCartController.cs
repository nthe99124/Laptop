using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
namespace Laptop.Controllers
{
    public class _clientCartController : Controller
    {
        LaptopNTT db = new LaptopNTT();
        // GET: _clientCart
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                ViewBag.Cart = null;
            }
            else
            {
                var Id_Cus = Convert.ToInt32(Session["ID_cus"]);
                ViewBag.Cart = from c in db.Carts
                               join pr in db.Product_Color on c.ID_Product_Color equals pr.ID
                               join pro in db.Products on pr.ID_Product equals pro.ID
                               join co in db.Colorrs on pr.ID_Color equals co.ID
                               where c.ID_Customer == Id_Cus
                               orderby c.created_at descending
                               select new GioHang
                               {
                                   Image = pro.Image,
                                   Name = pro.Name,
                                   Color = co.Color,
                                   Price = (int)pro.Promotion_Price,
                                   ID = (int)c.ID,
                                   ID_pro = (int)pro.ID,
                                   Quantity_Purchased = (int)c.Quantity_Purchased,
                                   Quantity = (int)pr.Quantity,
                                   Total_Price = (int)pro.Promotion_Price * (int)c.Quantity_Purchased
                               };
            }
            return View();
        }
        public ActionResult Add_To_Card(Cart ca)
        {

            int key = Convert.ToInt32(Request["key"]);
            ViewBag.date = DateTime.Now;
            Cart c = db.Carts.Where(a => a.ID_Product_Color == key).SingleOrDefault();
            if (Session["user"] == null)
            {
                ViewBag.test = "Bạn cần đăng nhập!";
            }
            else if (c != null)
            {
                c.Quantity_Purchased = c.Quantity_Purchased + 1;
                c.created_at = ViewBag.date;
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ca.ID_Product_Color = key;
                ca.ID_Customer = Convert.ToInt32(Session["ID_cus"]);
                ca.Quantity_Purchased = 1;
                ca.created_at = ViewBag.date;
                db.Carts.Add(ca);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "_clientProductDetail", new { key = Convert.ToInt32(Session["load"]) });
        }
        public ActionResult Delete(Cart ca)
        {
            int key = Convert.ToInt32(Request["key"]);
            ca = db.Carts.Where(c => c.ID == key).SingleOrDefault();
            db.Carts.Remove(ca);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Update(Cart c, int id)
        {
            var ca = from a in db.Carts
                     select a.ID;
            foreach (var item in ca)
            {
                c = db.Carts.Where(p => p.ID == id).SingleOrDefault();
                int update = Convert.ToInt32(Request["quan"]);
                ViewBag.date = DateTime.Now;
                c.Quantity_Purchased = update + 1;
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Updatet()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Updatet(Cart c, int id)
        {
            var ca = from a in db.Carts
                     select a.ID;
            foreach (var item in ca)
            {
                c = db.Carts.Where(p => p.ID == id).SingleOrDefault();
                int update = Convert.ToInt32(Request["quan"]);
                ViewBag.date = DateTime.Now;
                c.Quantity_Purchased = update - 1;
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult pay()
        {
            return View();
        }
        [HttpPost]
        public ActionResult pay(FormCollection data)
        {

            ViewBag.Cart = from c in db.Carts
                           join pr in db.Product_Color on c.ID_Product_Color equals pr.ID
                           join pro in db.Products on pr.ID_Product equals pro.ID
                           join co in db.Colorrs on pr.ID_Color equals co.ID
                           where c.ID_Customer.Equals(Session["ID_cus"])
                           orderby c.created_at descending
                           select new GioHang
                           {
                               Image = pro.Image,
                               Name = pro.Name,
                               Color = co.Color,
                               Price = (int)pro.Promotion_Price,
                               ID = (int)c.ID,
                               ID_pro = (int)pro.ID,
                               Quantity_Purchased = (int)c.Quantity_Purchased,
                               Quantity = (int)pr.Quantity,
                               Total_Price = (int)pro.Promotion_Price * (int)c.Quantity_Purchased
                           };
            Session["sdt"] = Convert.ToInt32(Request["phone"]);
            Session["add"] = Request["add"];
            return View();
        }
        public ActionResult Bill(Bill b)
        {
            b.Date_order = DateTime.Now;
            b.ID_Customer = Convert.ToInt32(Session["ID_cus"]);
            b.Address = Convert.ToString(Session["add"]);
            b.Phone_Number = Convert.ToString(Session["sdt"]);
            b.Confirm = "Chờ xác nhận";
            db.Bills.Add(b);
            db.SaveChanges();
            Bill bill = db.Bills.OrderByDescending(m => m.Date_order).Take(1).SingleOrDefault();
            var ca = from a in db.Carts
                     select a;
            var pr = from a in db.Products
                     select a;
            var ca_pr = from a in db.Carts
                        join pro_co in db.Product_Color on a.ID_Product_Color equals pro_co.ID
                        join pro in db.Products on pro_co.ID_Product equals pro.ID
                        select new GioHang
                        {
                            ID = a.ID,
                            Order_Price = (int)pro.Promotion_Price
                        };

            ViewBag.Cus_Detail = (from bd in db.Bill_Detail
                                  join bc in db.Bills on bd.ID_Bill equals bc.ID
                                  join cus in db.Customers on b.ID_Customer equals cus.ID
                                  where bd.ID_Bill == bill.ID
                                  select new Bill_Detaill
                                  {
                                      ID_Bill = bc.ID,
                                      Cus_Name = cus.Name,
                                      Cus_Email = cus.Email,
                                      Cus_Phone = bc.Phone_Number,
                                      Bill_Add = bc.Address
                                  }).Distinct();
            ViewBag.bill_detail = (from bd in db.Bill_Detail
                                   join bc in db.Bills on bd.ID_Bill equals b.ID
                                   join cus in db.Customers on b.ID_Customer equals cus.ID
                                   join pro_co in db.Product_Color on bd.ID_Product_Color equals pro_co.ID
                                   join pro in db.Products on pro_co.ID_Product equals pro.ID
                                   join bra in db.Brands on pro.ID_Brand equals bra.ID
                                   join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                   where bc.ID == bill.ID
                                   select new Bill_Detaill
                                   {
                                       ID = bd.ID,
                                       ID_Bill = bc.ID,
                                       Quantity = (int)bd.Quantity,
                                       Cus_Name = cus.Name,
                                       Cus_Email = cus.Email,
                                       Bill_Add = bc.Address,
                                       Cus_Phone = bc.Phone_Number,
                                       Cus_Gender = cus.Gender,
                                       Pro_Name = pro.Name,
                                       Pro_Brand = bra.Name,
                                       Order_Price = (decimal)bd.order_price,
                                       Pro_Color = co.Color
                                   }).Distinct();
            foreach (var item in ca)
            {
                Bill_Detail bi = new Bill_Detail();
                bi.ID_Bill = bill.ID;
                bi.ID_Product_Color = item.ID_Product_Color;
                foreach (var it in ca_pr)
                {
                    if (it.ID == item.ID)
                        bi.order_price = it.Order_Price;

                }
                bi.Quantity = item.Quantity_Purchased;
                db.Bill_Detail.Add(bi);
                db.Carts.Remove(item);
                db.SaveChanges();
            }
            return View();

        }
        public ActionResult Buy()
        {
            int key = Convert.ToInt32(Request["key"]);
            Session["key"] = Convert.ToInt32(Request["key"]);
            ViewBag.Cart = from c in db.Carts
                           join pr in db.Product_Color on c.ID_Product_Color equals pr.ID
                           join pro in db.Products on pr.ID_Product equals pro.ID
                           join co in db.Colorrs on pr.ID_Color equals co.ID
                           where c.ID_Customer.Equals(Session["ID_cus"])
                           orderby c.created_at descending
                           select new GioHang
                           {
                               Image = pro.Image,
                               Name = pro.Name,
                               Color = co.Color,
                               Price = (int)pro.Promotion_Price,
                               ID = (int)c.ID,
                               ID_pro = (int)pro.ID,
                               Quantity_Purchased = (int)c.Quantity_Purchased,
                               Quantity = (int)pr.Quantity,
                               Total_Price = (int)pro.Promotion_Price * (int)c.Quantity_Purchased
                           };
            ViewBag.cus = from cus in db.Customers
                          where cus.ID == (int)Session["ID_cus"]
                          select cus;

            return View();
        }

    }
}