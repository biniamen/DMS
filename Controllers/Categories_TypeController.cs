using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShareSysProd.Models;

namespace ShareSysProd.Controllers
{
    public class Categories_TypeController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();

        // GET: Categories_Type
        public ActionResult Index()
        {
            var categories_Type = db.Categories_Type.Include(c => c.AspNetUser).Include(c => c.AspNetUser1);
            return View(categories_Type.ToList());
        }

        // GET: Categories_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories_Type categories_Type = db.Categories_Type.Find(id);
            if (categories_Type == null)
            {
                return HttpNotFound();
            }
            return View(categories_Type);
        }

        // GET: Categories_Type/Create
        public ActionResult Create()
        {
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Categories_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoriesId,CategoryName,Description,CreatedDate,ApprovedDate,CreatedByUserId,ApprovedByUserId")] Categories_Type categories_Type)
        {
            if (ModelState.IsValid)
            {
                string currentUser = User.Identity.GetUserId();
                categories_Type.CreatedDate = DateTime.Now;
                categories_Type.CreatedByUserId = currentUser;
                db.Categories_Type.Add(categories_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", categories_Type.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", categories_Type.CreatedByUserId);
            return View(categories_Type);
        }

        // GET: Categories_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories_Type categories_Type = db.Categories_Type.Find(id);
            if (categories_Type == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", categories_Type.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", categories_Type.CreatedByUserId);
            return View(categories_Type);
        }

        // POST: Categories_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoriesId,CategoryName,Description,CreatedDate,ApprovedDate,CreatedByUserId,ApprovedByUserId")] Categories_Type categories_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categories_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", categories_Type.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", categories_Type.CreatedByUserId);
            return View(categories_Type);
        }

        // GET: Categories_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories_Type categories_Type = db.Categories_Type.Find(id);
            if (categories_Type == null)
            {
                return HttpNotFound();
            }
            return View(categories_Type);
        }

        // POST: Categories_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories_Type categories_Type = db.Categories_Type.Find(id);
            db.Categories_Type.Remove(categories_Type);
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
    }
}
