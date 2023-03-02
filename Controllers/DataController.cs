using ASP.Data;
using ASP.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ASP.Controllers
{
    [Authorize()]
    public class DataController : Controller
    {
        // GET: Data
        DataServices Data = new DataServices();
        [HttpGet]
        public ActionResult Schedulers()
        {
            AS_DBEntities db = new AS_DBEntities();
            int id = Int32.Parse(User.Identity.Name);

            //var Items = db.Schedulers.Where(Sh => Sh.userId == id).ToList().OrderBy(Sh => Sh.EndDate);
            var Items = db.Schedulers.Where(Sh => Sh.userId == id).ToList();

            //List<Scheduler> Items=new List<Scheduler>();
            //foreach (var item in db.Schedulers.ToList()){
            //    if(item.userId == id)
            //    {
            //        Items.Add(item);
            //    }
            //if (DateTime.Compare(item.EndDate, DateTime.Now) < 1)
            //{
            //    SendEmail(item, id);
            //}
        //}

        Items.Sort();
            return View(Items);
        }
        [HttpPost]
        public ActionResult Schedulers(string Name)
        {
            AS_DBEntities db = new AS_DBEntities();
            int id = Int32.Parse(User.Identity.Name);

            Name = "%" + Name + "%";
            
            var Items = db.Schedulers.Where(Sh =>
                        (SqlFunctions.PatIndex(Name.ToLower(), Sh.Name.ToLower()) > 0)
                        && (id == Sh.userId))
                        .ToList();

            Items.Sort();
            return View(Items);
        }
        [Authorize]
               
        public ActionResult Create() {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,description,EndDate")] Scheduler schedulers)
        {
            if (ModelState.IsValid)
            {
                schedulers.userId = Int32.Parse(User.Identity.Name);
                int check = Data.Add(schedulers);
                if (check == 1)
                {
                    return RedirectToAction("Schedulers");
                }
                if (check == 0 || check == -2)
                {
                    ModelState.AddModelError("EndDate", "invalid Data");
                }
                if (check == -1 || check == -2)
                {
                    ModelState.AddModelError("Name", "invalid Data");
                }
            }
            return View(schedulers);
        }

        public ActionResult Edit(int? id)
        {
            AS_DBEntities db = new AS_DBEntities();
            if (id == null || !db.Schedulers.Where(MM=>MM.id==id).Any())
                return View("Error");
            return View(db.Schedulers.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(Scheduler schedulers)
        {
            bool check=Data.Edit(schedulers, Int32.Parse(User.Identity.Name));
            if(check)
            {
                return RedirectToAction("Schedulers");
            }
            else
            {
                return View("Error");
            }
        }
        public ActionResult Details(int? id)
        {
            AS_DBEntities db = new AS_DBEntities();
            if (id == null || !db.Schedulers.Where(MM => MM.id == id).Any())
                return View("Error");

            return View(db.Schedulers.Find(id));
        }
        public ActionResult Delete(int? id)
        {
            bool check=Data.Delete(id, Int32.Parse(User.Identity.Name));
            if(check)
            {
                return RedirectToAction("Schedulers");
            }
            else
            {
                return View("Error");
            }
        }
        public void SendEmail(Scheduler scheduler, int? id)
        {
            AS_DBEntities db = new AS_DBEntities();
            var senderEmail = new MailAddress("abd.allh.dghemy1234@gmail.com", "Jamil");
            var receiverEmail = new MailAddress(db.Accounts.Find(id).Email, "Receiver");
            var password = "A01123882500";
            var subject = @"scheduler";
            var body = "your scheduler come over and we send massage for remeber u by ur scheduler";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(mess);
            }
        }

    } 
}