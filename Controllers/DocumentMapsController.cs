using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareSysProd.Models;

namespace ShareSysProd.Controllers
{
    public class DocumentMapsController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();

        // GET: DocumentMaps
        public ActionResult Index()
        {
            var documentMaps = db.DocumentMaps.Include(d => d.Categories_Type).Include(d => d.DocumentType);
            return View(documentMaps.ToList());
        }

        // GET: DocumentMaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentMap documentMap = db.DocumentMaps.Find(id);
            if (documentMap == null)
            {
                return HttpNotFound();
            }
            return View(documentMap);
        }

        // GET: DocumentMaps/Create
        public ActionResult Create()
        {
            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName");
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "DocumentTypeName");
            return View();
        }

        // POST: DocumentMaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocumentMapId,CategoriesId,DocumentTypeId,Description,Status,Mandatorie,CreatedDate,ApprovedDate")] DocumentMap documentMap)
        {
            if (ModelState.IsValid)
            {
                documentMap.CreatedDate = DateTime.Now;
                documentMap.ApprovedDate = DateTime.Now;
                documentMap.Status = "Approved";
                db.DocumentMaps.Add(documentMap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            

            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName", documentMap.CategoriesId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "DocumentTypeName", documentMap.DocumentTypeId);
            return View(documentMap);
        }

        // GET: DocumentMaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentMap documentMap = db.DocumentMaps.Find(id);
            if (documentMap == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName", documentMap.CategoriesId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "DocumentTypeName", documentMap.DocumentTypeId);
            return View(documentMap);
        }

        // POST: DocumentMaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocumentMapId,CategoriesId,DocumentTypeId,Description,Status,Mandatorie,CreatedDate,ApprovedDate")] DocumentMap documentMap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentMap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName", documentMap.CategoriesId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "DocumentTypeName", documentMap.DocumentTypeId);
            return View(documentMap);
        }

        // GET: DocumentMaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentMap documentMap = db.DocumentMaps.Find(id);
            if (documentMap == null)
            {
                return HttpNotFound();
            }
            return View(documentMap);
        }

        // POST: DocumentMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentMap documentMap = db.DocumentMaps.Find(id);
            db.DocumentMaps.Remove(documentMap);
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
