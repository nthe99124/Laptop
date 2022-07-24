using Laptop.Common.DataType;
using Laptop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Customer = Laptop.Common.DataType.Customer;

namespace Laptop.Controllers
{
    public class ClientBillController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: _clientBill
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buy()
        {
            var idCus = Convert.ToInt32(Session["ID_cus"]);
            var key = Convert.ToInt32(Request["key"]);
            Session["key"] = Convert.ToInt32(Request["key"]);
            ViewBag.cus = from cus in _db.Customers
                          where cus.ID == idCus
                          select cus;
            ViewBag.color = from pro in _db.Products
                            join prCo in _db.Product_Color on pro.ID equals prCo.ID_Product
                            join co in _db.Colors on prCo.ID_Color equals co.ID
                            where pro.ID == key
                            select new client_bill
                            {
                                Pro_Color = co.Color,
                                ID_Pro_Co = prCo.ID
                            };
            ViewBag.pro = (from pro in _db.Products
                           join prCo in _db.Product_Color on pro.ID equals prCo.ID_Product
                           join br in _db.Brands on pro.ID_Brand equals br.ID
                           join co in _db.Colors on prCo.ID_Color equals co.ID
                           where pro.ID == key
                           select new client_bill
                           {
                               Pro_ID = pro.ID,
                               Pro_Image = pro.Image,
                               Pro_Name = pro.Name,
                               Pro_Brand = br.Name,
                               Pro_Price = (int)pro.Promotion_Price

                           }).Take(1);

            return View();
        }

        public ActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pay(Product pro, int id)
        {
            var proColor = Convert.ToInt32(Request["Pro_Color"]);
            pro = _db.Products.SingleOrDefault(m => m.ID == id);
            ViewBag.color = from pr in _db.Products
                            join prCo in _db.Product_Color on pr.ID equals prCo.ID_Product
                            join co in _db.Colors on prCo.ID_Color equals co.ID
                            where prCo.ID == proColor
                            select new client_bill
                            {
                                Pro_Color = co.Color,
                            };
            Session["sdt"] = Request["phone"];
            Session["add"] = Request["add"];
            if (pro != null)
            {
                Session["image"] = pro.Image;
                Session["pro_name"] = pro.Name;
                Session["color"] = Request["Pro_Color"];
                Session["quan"] = Request["quan"];
                Session["price"] = pro.Promotion_Price;
            }
            return View();
        }

        public ActionResult Bill()
        {
            Session["color"] = null;
            var bill = _db.Bills.OrderByDescending(m => m.Date_order).Take(1).SingleOrDefault();
            ViewBag.bill_detail = (from bd in _db.Bill_Detail
                                   join b in _db.Bills on bd.ID_Bill equals b.ID
                                   join cus in _db.Customers on b.ID_Customer equals cus.ID
                                   join proCo in _db.Product_Color on bd.ID_Product_Color equals proCo.ID
                                   join pro in _db.Products on proCo.ID_Product equals pro.ID
                                   join bra in _db.Brands on pro.ID_Brand equals bra.ID
                                   join co in _db.Colors on proCo.ID_Color equals co.ID
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

            ViewBag.Cus_Detail = (from bd in _db.Bill_Detail
                                  join b in _db.Bills on bd.ID_Bill equals b.ID
                                  join cus in _db.Customers on b.ID_Customer equals cus.ID
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
            if (Session["color"] == null) return View();
            b.Date_order = DateTime.Now;
            b.ID_Customer = Convert.ToInt32(Session["ID_cus"]);
            b.Address = Convert.ToString(Session["add"]);
            b.Phone_Number = Convert.ToString(Session["sdt"]);
            b.Status = BillStatusConstant.WaitForConfirmation;
            _db.Bills.Add(b);
            _db.SaveChanges();
            Bill bill = _db.Bills.OrderByDescending(m => m.Date_order).Take(1).SingleOrDefault();

            if (bill != null) bi.ID_Bill = bill.ID;
            bi.ID_Product_Color = Convert.ToInt32(Session["color"]);
            bi.Quantity = Convert.ToInt32(Session["quan"]);
            bi.order_price = Convert.ToInt32(Session["price"]);
            _db.Bill_Detail.Add(bi);
            _db.SaveChanges();
            return this.Bill();
        }

        public ActionResult Bill_cus()
        {
            var bill = from b in _db.Bills
                       where b.ID_Customer == Convert.ToInt32(Session["ID_cus"])
                       orderby b.Date_order descending
                       select b;
            var idCus = Convert.ToInt32(Session["ID_cus"]);
            ViewBag.name = (from b in _db.Bills
                            join cus in _db.Customers on b.ID_Customer equals cus.ID
                            where b.ID_Customer == idCus
                            orderby b.Date_order descending
                            select new Bill_Detaill
                            {
                                Cus_Name = cus.Name
                            }).Distinct();
            return View(bill);
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
                                       Pro_Color = co.Color1
                                   }).Distinct();
            ViewBag.Cus_Detail = (from bd in _db.Bill_Detail
                                  join b in _db.Bills on bd.ID_Bill equals b.ID
                                  join cus in _db.Customers on b.ID_Customer equals cus.ID
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
            var id = Convert.ToInt32(Request["id"]);
            var conf = (Request["confirm"]);
            bill = _db.Bills.SingleOrDefault(b => b.ID == id);
            if (bill != null)
            {
                bill.Status = conf;
                _db.Entry(bill).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Bill_cus");
        }

        public ActionResult Rate()
        {
            var id = Convert.ToInt32(Request["id"]);
            Session["id_bd"] = id;
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
            ViewBag.Cus_Detail = (from bd in _db.Bill_Detail
                                  join b in _db.Bills on bd.ID_Bill equals b.ID
                                  join cus in _db.Customers on b.ID_Customer equals cus.ID
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
        public ActionResult Rate(Bill_Detail bill_de)
        {
            var key = Convert.ToInt32(Request["key"]);
            var id = Convert.ToInt32(Session["id_bd"]);
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
            ViewBag.Cus_Detail = (from bd in _db.Bill_Detail
                                  join b in _db.Bills on bd.ID_Bill equals b.ID
                                  join cus in _db.Customers on b.ID_Customer equals cus.ID
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

            bill_de = _db.Bill_Detail.SingleOrDefault(a => a.ID == key);
            if (bill_de != null)
            {
                bill_de.comment = (Request["comment"]);
                bill_de.ratetime = DateTime.Now;
                bill_de.rate = Convert.ToInt32(Request["star"]);

                _db.Entry(bill_de).State = EntityState.Modified;
            }
            _db.SaveChanges();
            return this.View();
        }
    }
}