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
    [Authorize(Roles = "Admin")]
    public class DocumentTypesController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();

        // GET: DocumentTypes
        public ActionResult Index()
        {
            var documentTypes = db.DocumentTypes.Include(d => d.AspNetUser).Include(d => d.AspNetUser1);
            return View(documentTypes.ToList());
        }

        // GET: DocumentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // GET: DocumentTypes/Create
        public ActionResult Create()
        {
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: DocumentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocumentTypeId,DocumentTypeName,Description,CreatedBy,CreatedDate,ApprovedBy,ApprovedDate,CreatedByUserId,ApprovedByUserId")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                string currentUser = User.Identity.GetUserId();
                documentType.CreatedDate = DateTime.Now;
                documentType.CreatedByUserId = currentUser;
                db.DocumentTypes.Add(documentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentType.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentType.CreatedByUserId);
            return View(documentType);
        }

        // GET: DocumentType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentType.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentType.CreatedByUserId);
            return View(documentType);
        }

        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: DocumentType/Edit/5
      
        public ActionResult Edit([Bind(Include = "DocumentTypeId,DocumentTypeName,Description,CreatedBy,CreatedDate,ApprovedBy,ApprovedDate,CreatedByUserId,ApprovedByUserId")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentType.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentType.CreatedByUserId);
            return View(documentType);
        }
        // GET: DocumentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // POST: DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentType documentType = db.DocumentTypes.Find(id);
            db.DocumentTypes.Remove(documentType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      
    }
}
