using Laptop.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: customer
        public ActionResult Index(int? page)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "LoginAdmin");
            }
            var cus = (from b in _db.Customers
                       select b).ToList();
            ViewBag.cus = cus;
            return View(cus.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Edit(int id)
        {
            var cus = _db.Customers.First(b => b.ID == id);
            return View(cus);
        }

        [HttpPost]
        public ActionResult Edit(int id, Customer cus)
        {
            ViewBag.date = DateTime.Now;
            cus = _db.Customers.SingleOrDefault(b => b.ID == id);
            if (cus != null)
            {
                cus.Status = Request["sta"];
                cus.Note = Request["note"];
                cus.updated_at = ViewBag.date;
                _db.Entry(cus).State = EntityState.Modified;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var cus = _db.Customers.SingleOrDefault(b => b.ID == id);
            if (cus != null) _db.Customers.Remove(cus);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}