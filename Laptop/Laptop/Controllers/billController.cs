using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using Laptop.Controllers;

namespace Laptop.Controllers
{
    public class billController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: bill
        public ActionResult Index(int? page)
        {
            if(Session["admin"]==null)
            {
                return RedirectToAction("Index", "loginAdmin");
            }    
            var bill = from b in db.Bills
                       orderby b.Date_order descending
                        select b;
            ViewBag.bill = from b in db.Bills
                           orderby b.Date_order descending
                           select b;
            return View(bill.ToPagedList(page ?? 1, 5));
        }/*
        [HttpPost]
        public ActionResult Index(int? page, FormCollection a)
        {
            DateTime date = Convert.ToDateTime( Request["key"]);
            var bill = from b in db.Bills
                        where b.Date_order==date
                        select b;
            return View(bill.ToPagedList(page ?? 1, 5));
        }*/
        public ActionResult Bill_Detail(int id)
        {
            ViewBag.bill_detail = (from bd in db.Bill_Details
                                  join b in db.Bills on bd.ID_Bill equals b.ID
                                  join cus in db.Customers on b.ID_Customer equals cus.ID
                                  join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                  join pro in db.Products on pro_co.ID_Product equals pro.ID
                                  join bra in db.Brands on pro.ID_Brand equals bra.ID
                                  join co in db.Colorrs on pro_co.ID_Color equals co.ID
                                  where bd.ID_Bill == id
                                  select new Bill_Detaill
                                  {
                                      ID = bd.ID,
                                      ID_Bill= b.ID,
                                      Quantity = (int)bd.Quantity,
                                      Cus_Name=cus.Name,
                                      Cus_Email=cus.Email,
                                      Bill_Add = b.Address,
                                      Cus_Phone= b.Phone_Number,
                                      Cus_Gender=cus.Gender,
                                      Pro_Name=pro.Name,
                                      Pro_Brand=bra.Name,
                                      Order_Price=(decimal)bd.order_price,
                                      Pro_Color= co.Color
                                  }).Distinct();
            ViewBag.Cus_Detail = (from bd in db.Bill_Details
                                   join b in db.Bills on bd.ID_Bill equals b.ID
                                   join cus in db.Customers on b.ID_Customer equals cus.ID
                                   /*join pro_co in db.Product_Colors on bd.ID_Product_Color equals pro_co.ID
                                   join pro in db.Products on pro_co.ID_Product equals pro.ID
                                   join bra in db.Brands on pro.ID_Brand equals bra.ID
                                   join co in db.Colorrs on pro_co.ID_Color equals co.ID*/
                                   where bd.ID_Bill == id
                                   select new Bill_Detaill
                                   {           
                                       ID_Bill= b.ID,
                                       Cus_Name = cus.Name,
                                       Cus_Email = cus.Email,
                                       Cus_Phone = b.Phone_Number,
                                       Bill_Add = b.Address,
                                       
                                   }).Distinct();
            return View();
        }
        public ActionResult Edit(int id)
        {
            var bill = db.Bills.First(b => b.ID == id);
            return View(bill);
        }
        [HttpPost]
        public ActionResult Edit(int id, Bill bill)
        {
            ViewBag.date = DateTime.Now;
            bill = db.Bills.Where(b => b.ID == id).SingleOrDefault();
            bill.Confirm = Request["confirm"];
            UpdateModel(bill);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}