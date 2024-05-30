using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShareSysProd.Models;

namespace ShareSysProd.Controllers
{
    [Authorize]
    public class AspNetRolesController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: AspNetRoles
        public ActionResult Index()
        {
            return View(db.AspNetRoles.ToList());
        }

        // GET: AspNetRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                db.AspNetRoles.Add(aspNetRole);
                db.SaveChanges();
                TempData["message"] = "Role Successfully Created !";
                return RedirectToAction("Index");
            }

            return View(aspNetRole);
        }

        // GET: AspNetRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetRole).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Role Successfully Edited !";
                return RedirectToAction("Index");
            }
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspNetRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

      
        public ActionResult ManageRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Getting Roles with users
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var administrators = from role in roleManager.Roles
                                 from userRoles in role.Users
                                 join user in userManager.Users on userRoles.UserId equals user.Id
                                 where role.Id == id
                                 select new Users_in_Role_ViewModel()
                                 {
                                     UserId = user.Id,
                                     Fullname = user.FullName,
                                     Email = user.Email,
                                     Role = role.Name,
                                     RoleID = role.Id
                                 };
            return View(administrators);
        }

        // Remove User From Roles
        public ActionResult Remove_User_fromRole(AspNetRole model, FormCollection formCollection)
        {
            string Role_ID = formCollection["RoleID"];            
            string[] UserID = formCollection["UserID"].Split(new char[] { ',' });
            
            foreach (string user_id in UserID)
            {
                var UsrID = user_id;
                //var RemoveUserRole = db.DeleteUserfromRole(UsrID, Role_ID);

            }
            TempData["UserExist"] = "User Successfully Removed from the Role";
            return RedirectToAction("Index", "AspNetRoles", new { id = Role_ID });
        }

    }
}
