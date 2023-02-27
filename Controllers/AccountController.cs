using ASP.Data;
using ASP.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ASP.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private UserOfApp user = new UserOfApp();
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register([Bind(Include =("Name,Email,Password"))]Account item)
        {

            bool ValidEmail = user.VaildEmail(item.Email);

            if (!ValidEmail)
            {
                ModelState.AddModelError("Email", "We have same Email enter anthor one");
            }
            
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            else
            {
                AS_DBEntities db = new AS_DBEntities();
                db.Accounts.Add(item);
                    db.SaveChanges(); 
                return RedirectToAction("login");
            }
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login([Bind(Include = ("Email,Password"))] Account item)
        {
            bool Valid = user.VaildUser(item.Email, item.password);
            if (Valid)
            {
                AS_DBEntities db = new AS_DBEntities();
                int Userid = db.Accounts.First(M => M.Email == item.Email).Id;
                var useridentity = new ClaimsIdentity(
                  new List<Claim>(){
                                new Claim(ClaimTypes.Name, Userid.ToString())
                  }, "taskscookiesforusers");
                Request.GetOwinContext().Authentication.SignIn(useridentity);

                
                return RedirectToAction("Schedulers", "Data");
            }
            else
            {
                ModelState.AddModelError("Email", "Email or Password is wrong");
                return View(item);
            }
        }
        public ActionResult logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}