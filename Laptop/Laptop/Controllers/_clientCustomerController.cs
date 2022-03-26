using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.Entity;

namespace Laptop.Controllers
{
    public class _clientCustomerController : Controller
    {
        LaptopNTT db = new LaptopNTT();
        // GET: _clientCustomer
        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
        public ActionResult AccountDetail()
        {
            int key = @Convert.ToInt32(Session["ID_cus"].ToString());
            var tb = (from c in db.Customers
                      where c.ID == key
                      select c).Take(1);
            return View(tb);
        }
        public ActionResult AccountEdit()
        {
            int key = @Convert.ToInt32(Session["ID_cus"].ToString());
            var tb = from c in db.Customers
                     where c.ID == key
                     select c;
            return View(tb);
        }
        [HttpPost]
        public ActionResult AccountEdit(Customer tk)
        {
            int key = @Convert.ToInt32(Session["ID_cus"].ToString());
            string phone = Convert.ToString(Request["phone"]);
            string gender = Request["gender"];
            string name = Request["name"];
            string add = Request["address"];
            tk = db.Customers.Where(m => m.ID == key).SingleOrDefault();
            if (tk != null)
            {
                tk.Name = name;
                tk.Gender = gender;
                tk.Address = add;
                tk.Phone_Number = phone;
                tk.Email = Session["email"].ToString();
                tk.Status = "Active";
                tk.updated_at = DateTime.Now;
                db.Entry(tk).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("notification", "_clientCustomer");
        }
        public ActionResult notification()
        {
            var tb = from c in db.Customers
                     where c.ID == Convert.ToInt32(Session["ID_cus"])
                     select c;
            return View(tb);
        }
        [HttpPost]
        public ActionResult DoiMK(Customer tk)
        {

            int key = @Convert.ToInt32(Session["ID_cus"].ToString());
            var tb = from c in db.Customers
                     where c.ID == Convert.ToInt32(Session["ID_cus"])
                     select c;
            Session["doimk"] = null;
            string Encodemkcu = Request["passcu"];
            string mkcu = EncodePassword(Encodemkcu);
            string Encodemkmoi1 = Request["passmoi1"];
            string mkmoi1 = EncodePassword(Encodemkmoi1);
            string Encodemkmoi2 = Request["passmoi2"];
            string mkmoi2 = EncodePassword(Encodemkmoi2);
            tk = db.Customers.Where(m => m.ID == key).SingleOrDefault();
            if (tk != null)
            {
                if (tk.Password == mkcu)
                {
                    Session["doimk"] = tk;
                }
                if (tk.Password == mkcu && mkmoi1 == mkmoi2)
                {
                    Session["doimk"] = tk;
                    tk.Password = mkmoi1;
                    tk.updated_at = DateTime.Now;
                    db.Entry(tk).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                return View(tb);
            }

            return View(tb);
        }
    }
}