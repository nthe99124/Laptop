using Laptop.Common.Constants;
using Laptop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
namespace Laptop.Controllers
{
    public class ClientCartController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: _clientCart
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                ViewBag.Cart = null;
            }
            else
            {
                var idCus = Convert.ToInt32(Session["ID_cus"]);
                ViewBag.Cart = from c in _db.Carts
                               join pr in _db.Product_Color on c.ID_Product_Color equals pr.ID
                               join pro in _db.Products on pr.ID_Product equals pro.ID
                               join co in _db.Colors on pr.ID_Color equals co.ID
                               where c.ID_Customer == idCus
                               orderby c.created_at descending
                               select new GioHang
                               {
                                   Image = pro.Image,
                                   Name = pro.Name,
                                   Color = co.Color,
                                   Price = (int)pro.Promotion_Price,
                                   ID = c.ID,
                                   ID_pro = pro.ID,
                                   Quantity_Purchased = (int)c.Quantity_Purchased,
                                   Quantity = (int)pr.Quantity,
                                   Total_Price = (int)pro.Promotion_Price * (int)c.Quantity_Purchased
                               };
            }
            return View();
        }

        public ActionResult AddToCard(Cart ca)
        {

            var key = Convert.ToInt32(Request["key"]);
            ViewBag.date = DateTime.Now;
            var c = _db.Carts.SingleOrDefault(a => a.ID_Product_Color == key);
            if (Session["user"] == null)
            {
                ViewBag.test = "Bạn cần đăng nhập!";
            }
            else if (c != null)
            {
                c.Quantity_Purchased = c.Quantity_Purchased + 1;
                c.created_at = ViewBag.date;
                _db.Entry(c).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                ca.ID_Product_Color = key;
                ca.ID_Customer = Convert.ToInt32(Session["ID_cus"]);
                ca.Quantity_Purchased = 1;
                ca.created_at = ViewBag.date;
                _db.Carts.Add(ca);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "ClientProductDetail", new { key = Convert.ToInt32(Session["load"]) });
        }

        public ActionResult Delete(Cart ca)
        {
            var key = Convert.ToInt32(Request["key"]);
            ca = _db.Carts.SingleOrDefault(c => c.ID == key);
            if (ca != null) _db.Carts.Remove(ca);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(Cart c, int id)
        {
            var ca = from a in _db.Carts
                     select a.ID;
            foreach (var item in ca)
            {
                c = _db.Carts.SingleOrDefault(p => p.ID == id);
                var update = Convert.ToInt32(Request["quan"]);
                ViewBag.date = DateTime.Now;
                if (c != null)
                {
                    c.Quantity_Purchased = update + 1;
                    _db.Entry(c).State = EntityState.Modified;
                }
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pay(FormCollection data)
        {

            ViewBag.Cart = from c in _db.Carts
                           join pr in _db.Product_Color on c.ID_Product_Color equals pr.ID
                           join pro in _db.Products on pr.ID_Product equals pro.ID
                           join co in _db.Colors on pr.ID_Color equals co.ID
                           where c.ID_Customer.Equals(Session["ID_cus"])
                           orderby c.created_at descending
                           select new GioHang
                           {
                               Image = pro.Image,
                               Name = pro.Name,
                               Color = co.Color,
                               Price = (int)pro.Promotion_Price,
                               ID = c.ID,
                               ID_pro = pro.ID,
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
            b.Confirm = BillStatusConstant.WaitForConfirmation;
            _db.Bills.Add(b);
            _db.SaveChanges();
            var bill = _db.Bills.OrderByDescending(m => m.Date_order).Take(1).SingleOrDefault();
            var ca = from a in _db.Carts
                     select a;
            var caPr = from a in _db.Carts
                       join proCo in _db.Product_Color on a.ID_Product_Color equals proCo.ID
                       join pro in _db.Products on proCo.ID_Product equals pro.ID
                       select new GioHang
                       {
                           ID = a.ID,
                           Order_Price = (int)pro.Promotion_Price
                       };

            ViewBag.Cus_Detail = (from bd in _db.Bill_Detail
                                  join bc in _db.Bills on bd.ID_Bill equals bc.ID
                                  join cus in _db.Customers on b.ID_Customer equals cus.ID
                                  where bd.ID_Bill == bill.ID
                                  select new Bill_Detaill
                                  {
                                      ID_Bill = bc.ID,
                                      Cus_Name = cus.Name,
                                      Cus_Email = cus.Email,
                                      Cus_Phone = bc.Phone_Number,
                                      Bill_Add = bc.Address
                                  }).Distinct();
            ViewBag.bill_detail = (from bd in _db.Bill_Detail
                                   join bc in _db.Bills on bd.ID_Bill equals b.ID
                                   join cus in _db.Customers on b.ID_Customer equals cus.ID
                                   join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                                   join pro in _db.Products on proCo.ID_Product equals pro.ID
                                   join bra in _db.Brands on pro.ID_Brand equals bra.ID
                                   join co in _db.Colors on proCo.ID_Color equals co.ID
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
            var bi = new Bill_Detail();
            foreach (var item in ca)
            {
                if (bill != null) bi.ID_Bill = bill.ID;
                bi.ID_Product_Color = item.ID_Product_Color;
                foreach (var it in caPr)
                {
                    if (it.ID == item.ID)
                        bi.order_price = it.Order_Price;

                }
                bi.Quantity = item.Quantity_Purchased;
                _db.Bill_Detail.Add(bi);
                _db.Carts.Remove(item);
                _db.SaveChanges();
            }
            return View();
        }

        public ActionResult Buy()
        {
            Session["key"] = Convert.ToInt32(Request["key"]);
            ViewBag.Cart = from c in _db.Carts
                           join pr in _db.Product_Color on c.ID_Product_Color equals pr.ID
                           join pro in _db.Products on pr.ID_Product equals pro.ID
                           join co in _db.Colors on pr.ID_Color equals co.ID
                           where c.ID_Customer.Equals(Session["ID_cus"])
                           orderby c.created_at descending
                           select new GioHang
                           {
                               Image = pro.Image,
                               Name = pro.Name,
                               Color = co.Color,
                               Price = (int)pro.Promotion_Price,
                               ID = c.ID,
                               ID_pro = pro.ID,
                               Quantity_Purchased = (int)c.Quantity_Purchased,
                               Quantity = (int)pr.Quantity,
                               Total_Price = (int)pro.Promotion_Price * (int)c.Quantity_Purchased
                           };
            ViewBag.cus = from cus in _db.Customers
                          where cus.ID == (int)Session["ID_cus"]
                          select cus;

            return View();
        }
    }
}