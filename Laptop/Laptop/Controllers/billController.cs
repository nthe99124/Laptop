using Laptop.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class BillController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: bill
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var bill = from b in _db.Bills
                       orderby b.Date_order descending
                       select b;
            ViewBag.bill = from b in _db.Bills
                           orderby b.Date_order descending
                           select b;
            return View(bill.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Bill_Detail(int id)
        {
            ViewBag.bill_detail = (from bd in _db.Bill_Detail
                                   join b in _db.Bills on bd.ID_Bill equals b.ID
                                   join cus in _db.Customers on b.ID_Customer equals cus.ID
                                   join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                                   join pro in _db.Products on proCo.ID_Product equals pro.ID
                                   join bra in _db.Brands on pro.ID_Brand equals bra.ID
                                   join co in _db.Colors on proCo.ID_Color equals co.ID
                                   where bd.ID_Bill == id
                                   select new Bill_Detaill
                                   {
                                       ID = bd.ID,
                                       ID_Bill = b.ID,
                                       Quantity = bd.Quantity,
                                       Cus_Name = cus.Name,
                                       Cus_Email = cus.Email,
                                       Bill_Add = b.Address,
                                       Cus_Phone = b.Phone_Number,
                                       Customer_Gender = cus.Gender,
                                       Pro_Name = pro.Name,
                                       Pro_Brand = bra.Name,
                                       Order_Price = bd.order_price,
                                       Pro_Color = co.Color
                                   }).Distinct();
            ViewBag.Cus_Detail = (from bd in _db.Bill_Detail
                                  join b in _db.Bills on bd.ID_Bill equals b.ID
                                  join cus in _db.Customers on b.ID_Customer equals cus.ID
                                  /*join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                  join pro in db.Products on pro_co.ID_Product equals pro.ID
                                  join bra in db.Brands on pro.ID_Brand equals bra.ID
                                  join co in db.Colors on pro_co.ID_Color equals co.ID*/
                                  where bd.ID_Bill == id
                                  select new Bill_Detaill
                                  {
                                      ID_Bill = b.ID,
                                      Cus_Name = cus.Name,
                                      Cus_Email = cus.Email,
                                      Cus_Phone = b.Phone_Number,
                                      Bill_Add = b.Address,

                                  }).Distinct();
            return View();
        }

        public ActionResult Edit(int id)
        {
            var bill = _db.Bills.First(b => b.ID == id);
            return View(bill);
        }

        [HttpPost]
        public ActionResult Edit(int id, Bill bill)
        {
            ViewBag.date = DateTime.Now;
            bill = _db.Bills.SingleOrDefault(b => b.ID == id);
            if (bill != null)
            {
                bill.Status = Request["confirm"] ==?;
                _db.Entry(bill).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}