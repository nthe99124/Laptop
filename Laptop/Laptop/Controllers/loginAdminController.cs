using Laptop.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
namespace Laptop.Controllers
{
    public class LoginAdminController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: loginAdmin
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
            if (Session["admin"] != null)
            {
                return RedirectToAction("Statistical", "Home");
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult Index(Admin tk)
        {
            var email = Request["Email"];
            var Encode = Request["PassWord"];
            var password = EncodePassword(Encode);

            tk = _db.Admins.SingleOrDefault(m => m.Email == email && m.Password == password);
            if (tk != null)
            {
                Session["admin"] = tk;
                Session["name"] = tk.Name;
                return RedirectToAction("Statistical", "Home");
            }
            else
            {
                ViewBag.error = "Email hoặc Password sai!";
                return this.Index();
            }
        }

        public ActionResult Login()
        {
            Session["admin"] = null;
            Session["name"] = null;
            return View();
        }
    }
}