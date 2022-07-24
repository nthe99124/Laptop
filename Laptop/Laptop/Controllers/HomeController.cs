using Laptop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Laptop.Common.Constants;
using Bill = Laptop.Common.DataType.Bill;
using Customer = Laptop.Common.DataType.Customer;

namespace Laptop.Controllers
{
    public class HomeController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();
        public ActionResult Index()
        {
            ViewBag.product = (from b in _db.Products
                               orderby b.ID descending
                               select b).Take(4);
            ViewBag.htvp = (from b in _db.Products
                            where b.Group_Pro == GroupProductConstant.StudyOffice
                            orderby b.ID descending
                            select b).Take(4);
            ViewBag.ccst = (from b in _db.Products
                            where b.Group_Pro == GroupProductConstant.Luxury
                            orderby b.ID descending
                            select b).Take(4);
            ViewBag.dhkt = (from b in _db.Products
                            where b.Group_Pro == GroupProductConstant.GraphicsEngineering
                            orderby b.ID descending
                            select b).Take(4);
            ViewBag.gaming = (from b in _db.Products
                              where b.Group_Pro == GroupProductConstant.GamingLaptop
                              orderby b.ID descending
                              select b).Take(4);

            return View();
        }

        public ActionResult Layout()
        {
            return View();
        }

        public ActionResult Statistical()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var date = DateTime.Now;
            ViewBag.billnow = (from b in _db.Bills
                               where b.Date_order.Value.Month.Equals(date.Month)
                               && b.Status.Equals(Bill.Status.Delivered) || b.Date_order.Value.Month.Equals(date.Month) && b.Status.Equals(Bill.Status.Received)
                               select b).Count();
            ViewBag.user = (from c in _db.Customers
                            where c.Status.Equals(Customer.Status.Active)
                            select c).Count();
            ViewBag.userlock = (from c in _db.Customers
                                select c).Count();
            ViewBag.dt = (from b in _db.Bills
                          join bd in _db.Bill_Detail on b.ID equals bd.ID_Bill
                          join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                          join pro in _db.Products on proCo.ID_Product equals pro.ID
                          where b.Date_order.Value.Month.Equals(date.Month)
                          && b.Date_order.Value.Year.Equals(date.Year) && b.Status.Equals(Bill.Status.Delivered)
                          || b.Date_order.Value.Month.Equals(date.Month) && b.Date_order.Value.Year.Equals(date.Year) && b.Status.Equals(Bill.Status.Received)
                          select new Bill_Detaill
                          {
                              Quantity = (int)bd.Quantity,
                              Order_Price = (decimal)bd.order_price
                          });

            ViewBag.dtn = (_db.Bills.Join(_db.Bill_Detail, b => b.ID, bd => bd.ID_Bill, (b, bd) => new { b, bd })
                .Join(_db.Product_Color, @t => @t.bd.ID_Product_Color, proCo => proCo.ID,
                    (@t, proCo) => new { @t, pro_co = proCo })
                .Join(_db.Products, @t => @t.pro_co.ID_Product, pro => pro.ID, (@t, pro) => new { @t, pro })
                .Where(@t =>
                    @t.@t.@t.b.Date_order.Value.Year.Equals(date.Year) && @t.@t.@t.b.Status.Equals(Bill.Status.Delivered) ||
                    @t.@t.@t.b.Date_order.Value.Year.Equals(date.Year) && @t.@t.@t.b.Status.Equals(Bill.Status.Received))
                .Select(@t => new Bill_Detaill
                {
                    Quantity = (int)@t.@t.@t.bd.Quantity,
                    Order_Price = (decimal)@t.@t.@t.bd.order_price
                }));
            ViewBag.sl = (from b in _db.Bills
                          join bd in _db.Bill_Detail on b.ID equals bd.ID_Bill
                          join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                          join pro in _db.Products on proCo.ID_Product equals pro.ID
                          where b.Date_order.Value.Month.Equals(date.Month)
                          && b.Date_order.Value.Year.Equals(date.Year) && b.Status.Equals(Bill.Status.Delivered)
                          || b.Date_order.Value.Month.Equals(date.Month) && b.Date_order.Value.Year.Equals(date.Year) && b.Status.Equals(Bill.Status.Received)
                          select new Bill_Detaill
                          {
                              Quantity = (int)bd.Quantity,
                          });
            ViewBag.slsp = (from pr in _db.Products
                            select pr).Count();
            ViewBag.slc = (from pr in _db.Product_Color
                           select pr);
            ViewBag.spbc = (from billDetail in _db.Bill_Detail
                            where billDetail.Bill.Date_order.Value.Month.Equals(date.Month) &&
                              billDetail.Bill.Date_order.Value.Year.Equals(date.Year) &&
                              billDetail.Bill.Status.Equals(Bill.Status.Received) ||
                              billDetail.Bill.Date_order.Value.Month.Equals(date.Month) &&
                              billDetail.Bill.Date_order.Value.Year.Equals(date.Year) &&
                              billDetail.Bill.Status.Equals(Bill.Status.Delivered)
                            group new { billDetail.Product_Color.Product, Bill_Detail = billDetail } by new
                            {
                                billDetail.Product_Color.Product.ID
                            } into g
                            orderby
                              g.Sum(p => p.Bill_Detail.Quantity) descending
                            select new Bill_Detaill
                            {
                                ID = g.Key.ID,
                                Quantity = (int)g.Sum(p => p.Bill_Detail.Quantity)
                            }).Take(5);
            ViewBag.pro = (from pr in _db.Products
                           select pr);
            return View();
        }

        public ActionResult Statistical_month(string month)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var date = DateTime.Now;
            if (!int.TryParse(month, out var monthOut) || monthOut > 12 || monthOut < 1)
            {
                monthOut = date.Month;
            }
            ViewBag.spbc = (_db.Bill_Detail
                .Where(billDetail =>
                    billDetail.Bill.Date_order.Value.Month.Equals(monthOut) &&
                    billDetail.Bill.Date_order.Value.Year.Equals(date.Year) &&
                    billDetail.Bill.Status.Equals(Bill.Status.Received) ||
                    billDetail.Bill.Date_order.Value.Month.Equals(monthOut) &&
                    billDetail.Bill.Date_order.Value.Year.Equals(date.Year) &&
                    billDetail.Bill.Status.Equals(Bill.Status.Delivered))
                .GroupBy(billDetail => new { billDetail.Product_Color.Product.ID },
                    billDetail => new { billDetail.Product_Color.Product, billDetail })
                .OrderByDescending(g => g.Sum(p => p.billDetail.Quantity))
                .Select(g => new Bill_Detaill { ID = g.Key.ID, Quantity = (int)g.Sum(p => p.billDetail.Quantity) })).Take(5);
            ViewBag.pro = (from pr in _db.Products
                           select pr);
            return View();
        }

        public JsonResult AjaxStatistical(int year)
        {
            var dt = new List<decimal>();
            for (var i = 0; i < 12; i++)
            {
                dt.Add(Doanhthuthang(i + 1, year));
            }
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public decimal Doanhthuthang(int month, int year)
        {
            var dt = (from b in _db.Bills
                      join bd in _db.Bill_Detail on b.ID equals bd.ID_Bill
                      join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                      join pro in _db.Products on proCo.ID_Product equals pro.ID
                      where b.Date_order.Value.Month.Equals(month) && b.Date_order.Value.Year.Equals(year) && b.Status.Equals(Bill.Status.Delivered)
                        || b.Date_order.Value.Month.Equals(month) && b.Date_order.Value.Year.Equals(year) && b.Status.Equals(Bill.Status.Received)
                      select new Bill_Detaill
                      {
                          Quantity = (int)bd.Quantity,
                          Order_Price = (decimal)bd.order_price
                      }).ToList();
            return dt.Aggregate<Bill_Detaill, decimal>(0, (current, item) => current + item.Order_Price * item.Quantity);
        }
    }
}