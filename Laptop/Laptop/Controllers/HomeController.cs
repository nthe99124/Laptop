using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;

namespace Laptop.Controllers
{
    public class HomeController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        public ActionResult Index()
        {
            ViewBag.product = (from b in db.Products
                               orderby b.ID descending
                              select b).Take(4);
            ViewBag.htvp = (from b in db.Products
                            where b.Group_Pro == "Học tập - Văn phòng"
                            orderby b.ID descending
                              select b).Take(4);
            ViewBag.ccst = (from b in db.Products
                            where b.Group_Pro == "Cao cấp - Sang trọng"
                            orderby b.ID descending
                              select b).Take(4);
            ViewBag.dhkt = (from b in db.Products
                            where b.Group_Pro == "Đồ họa - Kỹ thuật"
                            orderby b.ID descending
                              select b).Take(4);
            ViewBag.gaming = (from b in db.Products
                              where b.Group_Pro=="Laptop Gaming"
                               orderby b.ID descending
                              select b).Take(4);
            
            return View();
        }
        public ActionResult layout()
        {
            return View();
        }
        public ActionResult Statistical()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            DateTime date = DateTime.Now;
            ViewBag.billnow = (from b in db.Bills
                            where b.Date_order.Value.Month.Equals(date.Month)
                            && b.Confirm=="Đã giao hàng" || b.Date_order.Value.Month.Equals(date.Month) && b.Confirm == "Đã nhận được hàng"
                               select b).Count();
            ViewBag.user = (from c in db.Customers
                            where c.Status=="Active"
                            select c).Count();
            ViewBag.userlock = (from c in db.Customers
                            select c).Count();
            ViewBag.dt = (from b in db.Bills
                          join bd in db.Bill_Details on b.ID equals bd.ID_Bill
                          join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                          join pro in db.Products on pro_co.ID_Product equals pro.ID
                          where b.Date_order.Value.Month.Equals(date.Month)
                          && b.Date_order.Value.Year.Equals(date.Year) && b.Confirm == "Đã giao hàng"
                          || b.Date_order.Value.Month.Equals(date.Month) && b.Date_order.Value.Year.Equals(date.Year) && b.Confirm == "Đã nhận được hàng"
                          select new Bill_Detaill
                          {
                              Quantity = (int) bd.Quantity,
                              Order_Price = (decimal)bd.order_price
                          });
            ViewBag.dtn = (from b in db.Bills
                          join bd in db.Bill_Details on b.ID equals bd.ID_Bill
                          join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                          join pro in db.Products on pro_co.ID_Product equals pro.ID
                          where b.Date_order.Value.Year.Equals(date.Year) && b.Confirm == "Đã giao hàng"
                          || b.Date_order.Value.Year.Equals(date.Year) && b.Confirm == "Đã nhận được hàng"
                           select new Bill_Detaill
                          {
                              Quantity = (int)bd.Quantity,
                              Order_Price = (decimal) bd.order_price
                          });
            ViewBag.sl = (from b in db.Bills
                          join bd in db.Bill_Details on b.ID equals bd.ID_Bill
                          join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                          join pro in db.Products on pro_co.ID_Product equals pro.ID
                          where b.Date_order.Value.Month.Equals(date.Month)
                          && b.Date_order.Value.Year.Equals(date.Year) && b.Confirm == "Đã giao hàng" 
                          || b.Date_order.Value.Month.Equals(date.Month) && b.Date_order.Value.Year.Equals(date.Year) && b.Confirm == "Đã nhận được hàng"
                          select new Bill_Detaill
                          {
                              Quantity = (int) bd.Quantity,
                          });
            ViewBag.slsp= (from pr in db.Products
                           select pr).Count();
            ViewBag.slc= (from pr in db.Product_Colors
                           select pr);
            ViewBag.spbc = (from Bill_Details in db.Bill_Details
                            where  Bill_Details.Bill.Date_order.Value.Month.Equals(date.Month) &&
                              Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                              Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                              Bill_Details.Bill.Date_order.Value.Month.Equals(date.Month) &&
                              Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                              Bill_Details.Bill.Confirm == "Đã giao hàng"
                            group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                            {
                                Bill_Details.Product_Color.Product.ID
                            } into g
                            orderby
                              g.Sum(p => p.Bill_Details.Quantity) descending
                            select new Bill_Detaill
                            {
                                ID=g.Key.ID,
                                Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                            }).Take(5);
            ViewBag.pro = (from pr in db.Products
                            select pr);
            return View();
        }
        public ActionResult Statistical_month(string month)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }
            DateTime date = DateTime.Now;
            
            ViewBag.spbc = (from Bill_Details in db.Bill_Details
                            where
                              Bill_Details.Bill.Date_order.Value.Month.Equals(date.Month) &&
                              Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                              Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                              Bill_Details.Bill.Date_order.Value.Month.Equals(date.Month) &&
                              Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                              Bill_Details.Bill.Confirm == "Đã giao hàng"
                            group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                            {
                                Bill_Details.Product_Color.Product.ID
                            } into g
                            orderby
                              g.Sum(p => p.Bill_Details.Quantity) descending
                            select new Bill_Detaill
                            {
                                ID = g.Key.ID,
                                Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                            }).Take(5);
            ViewBag.pro = (from pr in db.Products
                           select pr);
            switch (month)
            {
                case "1":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month==1 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month==1 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "2":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 2 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 2 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "3":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 3 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 3 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "4":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 4 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 4 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "5":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 5 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 5 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "6":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 6 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 6 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "7":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 7 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 7 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);

                    break;
                case "8":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 8 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 8 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "9":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 9 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 9 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "10":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 10 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 10 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "11":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 11 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 11 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);
                    break;
                case "12":
                    ViewBag.spbc = (from Bill_Details in db.Bill_Details
                                    where
                                      Bill_Details.Bill.Date_order.Value.Month == 12 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã nhận được hàng" ||
                                      Bill_Details.Bill.Date_order.Value.Month == 12 &&
                                      Bill_Details.Bill.Date_order.Value.Year.Equals(date.Year) &&
                                      Bill_Details.Bill.Confirm == "Đã giao hàng"
                                    group new { Bill_Details.Product_Color.Product, Bill_Details } by new
                                    {
                                        Bill_Details.Product_Color.Product.ID
                                    } into g
                                    orderby
                                      g.Sum(p => p.Bill_Details.Quantity) descending
                                    select new Bill_Detaill
                                    {
                                        ID = g.Key.ID,
                                        Quantity = (int)g.Sum(p => p.Bill_Details.Quantity)
                                    }).Take(5);

                    break;
                default:
                    break;
            }
            return View();
        }
        public JsonResult AjaxStatistical(int year)
        {
            List<decimal> dt = new List<decimal>();
            for (int i = 0; i < 12; i++)
            {
                dt.Add(Doanhthuthang(i + 1, year));
            }
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public decimal Doanhthuthang(int month, int year)
        {
            var dt = (from b in db.Bills
                      join bd in db.Bill_Details on b.ID equals bd.ID_Bill
                      join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                      join pro in db.Products on pro_co.ID_Product equals pro.ID
                      where b.Date_order.Value.Month.Equals(month) && b.Date_order.Value.Year.Equals(year) && b.Confirm == "Đã giao hàng" 
                        || b.Date_order.Value.Month.Equals(month) && b.Date_order.Value.Year.Equals(year) && b.Confirm == "Đã nhận được hàng"
                      select new Bill_Detaill
                      {
                          Quantity= (int) bd.Quantity,
                          Order_Price = (decimal)bd.order_price
                      }).ToList();
            decimal dtt = 0;
            foreach (var item in dt)
            {
                dtt = dtt + item.Order_Price * item.Quantity;
            }
            return dtt;
        }
    }
}