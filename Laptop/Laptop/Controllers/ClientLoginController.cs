using Laptop.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
namespace Laptop.Controllers
{
    public class ClientLoginController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: _clientLogin
        private static string EncodePassword(string originalPassword)
        {
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)

            var md5 = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.Default.GetBytes(originalPassword);
            var encodedBytes = md5.ComputeHash(originalBytes);

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
            var email = Request["Email"];
            var encode = Request["PassWord"];
            var password = EncodePassword(encode);
            tk = _db.Customers.FirstOrDefault(m => m.Email == email && m.Password == password);
            if (tk != null)
            {
                if (tk.Status == "Active")
                {
                    Session["user"] = tk;
                    Session["Name"] = tk.Name;
                    Session["email"] = tk.Email;
                    Session["ID_cus"] = tk.ID;
                    return RedirectToAction("Index", "Home");
                }
                else if (tk.Status == "Lock")
                {
                    ViewBag.error = "Tài khoản của bạn đã bị khóa!";
                }
            }
            else
                ViewBag.error = "Email hoặc Password sai!";
            return this.Index();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer tk)
        {
            var phone = Convert.ToString(Request["phone"]);
            var email = Request["email"];
            var encode = Request["password"];
            var password = EncodePassword(encode);
            var gender = Request["gender"];
            var name = Request["name"];
            var add = Request["address"];
            var tk1 = _db.Customers.FirstOrDefault(m => m.Email == email);
            if (tk1 != null)
            {
                ViewBag.error = "Đã tồn tại tài khoản có Email: " + email;
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
                _db.Customers.Add(tk);
                _db.SaveChanges();
                Session["ID_cus"] = tk.ID;
                return RedirectToAction("Notification", "ClientLogin");
            }
            return View();
        }

        public ActionResult Notification()
        {
            var tb = from c in _db.Customers
                     where c.ID == Convert.ToInt32(Session["ID_cus"])
                     select c;
            return View(tb);
        }

        public ActionResult Logout()
        {
            if (Session["user"] != null)
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