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
    public class loginAdminController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: loginAdmin
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
            if(Session["admin"]!=null)
            {
                return RedirectToAction("Statistical", "Home");
            }
            else
            return View();
        }
        [HttpPost]
        public ActionResult Index(Admin tk)
        {
            
            string email = Request["Email"];
            string Encode = Request["PassWord"];
            string password = EncodePassword(Encode);

            tk = db.Admins.Where(m => m.Email == email && m.Password == password).SingleOrDefault();
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
                
            var dn = from a in db.Admins
                     where a.Email.Equals(email) && a.Password.Equals(password)
                     select a;
            ViewBag.dn = dn;
            return this.Index();
        }
        public ActionResult login()
        {
            Session["admin"] = null;
            Session["name"] = null;
            return View();            
        }

    }
}