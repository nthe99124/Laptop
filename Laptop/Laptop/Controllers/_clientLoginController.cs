using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptop.Models;
using System.Security.Cryptography;
using System.Text;
namespace Laptop.Controllers
{
    public class _clientLoginController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: _clientLogin
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
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Customer tk)
        {
            string email = Request["Email"];
            string Encode = Request["PassWord"];
            string password = EncodePassword(Encode);
            tk = db.Customers.Where(m => m.Email == email && m.Password == password).FirstOrDefault();
            if(tk != null&&tk.Status== "Active")
            {
                Session["user"] = tk;
                Session["Name"] = tk.Name;
                Session["email"] = tk.Email;
                Session["ID_cus"] = tk.ID;
                return RedirectToAction("Index", "Home");
            }
            else if (tk != null && tk.Status == "Lock")
            {
                ViewBag.error = "Tài khoản của bạn đã bị khóa!";
            }
            else
                ViewBag.error = "Email hoặc Password sai!";
            return this.Index();
        }
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(Customer tk)
        {
            string phone = Convert.ToString(Request["phone"]);
            string email = Request["email"];
            string Encode = Request["password"];
            string password = EncodePassword(Encode);
            string gender = Request["gender"];
            string name = Request["name"];
            string add = Request["address"];
            Customer tk1 = db.Customers.Where(m => m.Email == email).FirstOrDefault();
            if (tk1 != null)
            {
                ViewBag.error ="Đã tồn tại tài khoản có Email: " +email;
            }
            else
            {
                tk.Name = name;
                tk.Gender = gender;
                tk.Address = add;
                tk.Phone_Number = phone;
                tk.Email = email;
                tk.Password = password;
                tk.Status = "Active";
                tk.created_at = DateTime.Now;
                db.Customers.InsertOnSubmit(tk);
                db.SubmitChanges();
                Session["ID_cus"] = tk.ID;
                return RedirectToAction("notification", "_clientLogin");
            }    
            return View();
        }
        public ActionResult notification()
        {
            var tb = from c in db.Customers
                     where c.ID == Convert.ToInt32(Session["ID_cus"])
                      select c;
            return View(tb);
        }
        public ActionResult logout()
        {
            if(Session["user"] !=null)
            {
                Session["user"] = null;
                Session["Name"] = null;
                Session["ID_cus"] = null;
                return RedirectToAction("Index", "Home");
            }    
            return View();
        }
    }
}