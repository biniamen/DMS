using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShareSysProd.Models;

namespace ShareSysProd.Controllers
{
    public class DocumentationsController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();

        // GET: Documentations
        public ActionResult Index()
        {
            var documentations = db.Documentations.Include(d => d.AspNetUser).Include(d => d.AspNetUser1).Include(d => d.Categories_Type);
            return View(documentations.ToList());
        }

        // GET: Documentations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documentation documentation = db.Documentations.Find(id);
            if (documentation == null)
            {
                return HttpNotFound();
            }
            return View(documentation);
        }

        // GET: Documentations/Create
        public ActionResult Create()
        {
            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName");
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "UserName");
            return View(new DocumentationViewModel());
        }

        // POST: Documentations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "DocumentationId,CIF,ClientName,Mobile,Branch,CustomerType,CategoriesId,CreatedDate,ApprovedDate,Description,Status,Reason,CreatedByUserId,ApprovedByUserId,DocumentReference")] Documentation documentation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Documentations.Add(documentation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentation.ApprovedByUserId);
        //    ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentation.CreatedByUserId);
        //    ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName", documentation.CategoriesId);
        //    return View(documentation);
        //}


        // POST: Documentation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var documentation = new Documentation
                {
                    CIF = model.CIF,
                    ClientName = model.ClientName,
                    Mobile = model.Mobile,
                    Branch = model.Branch,
                    CustomerType = model.CustomerType,
                    CategoriesId = model.CategoriesId,
                    CreatedDate = DateTime.Now,
                    ApprovedDate = model.ApprovedDate,
                    Description = model.Description,
                    Status = model.Status,
                    Reason = model.Reason,
                    CreatedByUserId = User.Identity.GetUserId(),
                    ApprovedByUserId = model.ApprovedByUserId,
                    DocumentReference = model.DocumentReference
                };

                db.Documentations.Add(documentation);
                db.SaveChanges(); // Save to get the DocumentationId

                if (model.DocumentFiles != null && model.DocumentFiles.Count > 0)
                {
                    string uploadPath = Server.MapPath("~/App_Data/uploads");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    foreach (var fileEntry in model.DocumentFiles)
                    {
                        HttpPostedFileBase file = fileEntry.Value;
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            string path = Path.Combine(uploadPath, fileName);
                            file.SaveAs(path);

                            var document = new Document
                            {
                                DocumentationId = documentation.DocumentationId,
                                DocumentTypeId = fileEntry.Key,
                                FileLocation = path,
                                CreatedDate = DateTime.Now,
                                CreatedByUserId = documentation.CreatedByUserId
                            };

                            db.Documents.Add(document);
                        }
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName", model.CategoriesId);
            return View(model);
        }





        public ActionResult GetDocumentTypesByCategory(int categoryId)
        {
            var documentTypes = db.DocumentMaps
                .Where(dm => dm.CategoriesId == categoryId)
                .Select(dm => new {
                    DocumentTypeId = dm.DocumentTypeId,
                    DocumentTypeName = dm.DocumentType.DocumentTypeName // Adjust this line to match your model
        })
                .ToList();

            return Json(documentTypes, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<int> GetRequiredDocumentTypes(int categoriesId)
        {
            return db.DocumentMaps
                     .Where(dm => dm.CategoriesId == categoriesId && dm.Mandatorie)
                     .Select(dm => dm.DocumentTypeId)
                     .ToList();
        }

        private string SaveFile(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
            file.SaveAs(path);
            return path;
        }


        // GET: Documentations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documentation documentation = db.Documentations.Find(id);
            if (documentation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentation.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentation.CreatedByUserId);
            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName", documentation.CategoriesId);
            return View(documentation);
        }

        // POST: Documentations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocumentationId,CIF,ClientName,Mobile,Branch,CustomerType,CategoriesId,CreatedDate,ApprovedDate,Description,Status,Reason,CreatedByUserId,ApprovedByUserId,DocumentReference")] Documentation documentation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApprovedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentation.ApprovedByUserId);
            ViewBag.CreatedByUserId = new SelectList(db.AspNetUsers, "Id", "Email", documentation.CreatedByUserId);
            ViewBag.CategoriesId = new SelectList(db.Categories_Type, "CategoriesId", "CategoryName", documentation.CategoriesId);
            return View(documentation);
        }

        // GET: Documentations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documentation documentation = db.Documentations.Find(id);
            if (documentation == null)
            {
                return HttpNotFound();
            }
            return View(documentation);
        }

        // POST: Documentations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Documentation documentation = db.Documentations.Find(id);
            db.Documentations.Remove(documentation);
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
