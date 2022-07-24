using Laptop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Laptop.Controllers
{
    public class ClientCustomerController : Controller
    {
        private readonly LaptopNTT _db = new LaptopNTT();

        // GET: _clientCustomer
        private static string EncodePassword(string originalPassword)
        {
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)

            var md5 = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.Default.GetBytes(originalPassword);
            var encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }

        public ActionResult AccountDetail()
        {
            var key = Convert.ToInt32(Session["ID_cus"].ToString());
            var tb = (from c in _db.Customers
                      where c.ID == key
                      select c).Take(1);
            return View(tb);
        }

        public ActionResult AccountEdit()
        {
            var key = @Convert.ToInt32(Session["ID_cus"].ToString());
            var tb = from c in _db.Customers
                     where c.ID == key
                     select c;
            return View(tb);
        }

        [HttpPost]
        public ActionResult AccountEdit(Customer tk)
        {
            var key = @Convert.ToInt32(Session["ID_cus"].ToString());
            var phone = Convert.ToString(Request["phone"]);
            var gender = Request["gender"];
            var name = Request["name"];
            var add = Request["address"];
            tk = _db.Customers.SingleOrDefault(m => m.ID == key);
            if (tk == null) return RedirectToAction("Notification", "ClientCustomer");
            tk.Name = name;
            tk.Gender = gender;
            tk.Address = add;
            tk.Phone_Number = phone;
            tk.Email = Session["email"].ToString();
            tk.Status = "Active";
            tk.updated_at = DateTime.Now;
            _db.Entry(tk).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Notification", "ClientCustomer");
        }

        public ActionResult Notification()
        {
            var tb = from c in _db.Customers
                     where c.ID == Convert.ToInt32(Session["ID_cus"])
                     select c;
            return View(tb);
        }

        [HttpPost]
        public ActionResult ChangePassWord(Customer tk)
        {
            var key = @Convert.ToInt32(Session["ID_cus"].ToString());
            var tb = from c in _db.Customers
                     where c.ID == Convert.ToInt32(Session["ID_cus"])
                     select c;
            Session["doimk"] = null;
            var encodeOldPass = Request["passcu"];
            var oldPass = EncodePassword(encodeOldPass);
            var encodeNewPass1 = Request["passmoi1"];
            var newPass1 = EncodePassword(encodeNewPass1);
            var encodeNewPass2 = Request["passmoi2"];
            var newPass2 = EncodePassword(encodeNewPass2);
            tk = _db.Customers.SingleOrDefault(m => m.ID == key);
            if (tk != null)
            {
                if (tk.Password == oldPass)
                {
                    Session["doimk"] = tk;
                }
                if (tk.Password == oldPass && newPass1 == newPass2)
                {
                    Session["doimk"] = tk;
                    tk.Password = newPass1;
                    tk.updated_at = DateTime.Now;
                    _db.Entry(tk).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            return View(tb);
        }
    }
}