using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using ShareSysProd.Models;

namespace ShareSysProd.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Branches_and_DepartmentController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();

        // GET: Branches_and_Department
        public ActionResult Index()
        {
            return View(db.Branches_and_Department.ToList());
        }

        // GET: Branches_and_Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branches_and_Department branches_and_Department = db.Branches_and_Department.Find(id);
            if (branches_and_Department == null)
            {
                return HttpNotFound();
            }
            return View(branches_and_Department);
        }

        // GET: Branches_and_Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Branches_and_Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "branch_id,Branch_department_Code,Branch_department_name")] Branches_and_Department branches_and_Department)
        {
            if (ModelState.IsValid)
            {
                db.Branches_and_Department.Add(branches_and_Department);
                db.SaveChanges();
                TempData["message"] = "Branch/Department Information Successfully Saved";
                return RedirectToAction("Index");
            }

            return View(branches_and_Department);
        }

        // GET: Branches_and_Department/Edit/5
        public ActionResult Edit(int? branch_id)
        {
            if (branch_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branches_and_Department branches_and_Department = db.Branches_and_Department.Find(branch_id);
            if (branches_and_Department == null)
            {
                return HttpNotFound();
            }
            return View(branches_and_Department);
        }

        // POST: Branches_and_Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "branch_id,Branch_department_Code,Branch_department_name")] Branches_and_Department branches_and_Department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branches_and_Department).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Branch / Department Information Updated !";
                return RedirectToAction("Index");
            }
            return View(branches_and_Department);
        }

        // GET: Branches_and_Department/Delete/5
        

        // POST: Branches_and_Department/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int branch_id)
        {
            Branches_and_Department branches_and_Department = db.Branches_and_Department.Find(branch_id);
            db.Branches_and_Department.Remove(branches_and_Department);
            db.SaveChanges();
            TempData["message"] = "Branch / Department Information Deleted !";
            return RedirectToAction("Index");
        }

        // new 
        public async Task<JsonResult> GetCustomerDetails2(string custNo)
        {
            var soapRequest = $@"
            <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:fcub='http://fcubs.ofss.com/service/FCUBSCustomerService'>
               <soapenv:Header/>
               <soapenv:Body>
                  <fcub:QUERYCUSTOMER_IOFS_REQ>
                     <fcub:FCUBS_HEADER>
                        <fcub:SOURCE>TLB</fcub:SOURCE>
                        <fcub:UBSCOMP>FCUBS</fcub:UBSCOMP>
                        <fcub:USERID>KACHADP</fcub:USERID>
                        <fcub:BRANCH>000</fcub:BRANCH>
                        <fcub:SERVICE>FCUBSCustomerService</fcub:SERVICE>
                        <fcub:OPERATION>QueryCustomer</fcub:OPERATION>
                        <fcub:ACTION>VIEW</fcub:ACTION>
                     </fcub:FCUBS_HEADER>
                     <fcub:FCUBS_BODY>
                        <fcub:Customer-IO>
                           <fcub:CUSTNO>{custNo}</fcub:CUSTNO>
                        </fcub:Customer-IO>
                     </fcub:FCUBS_BODY>
                  </fcub:QUERYCUSTOMER_IOFS_REQ>
               </soapenv:Body>
            </soapenv:Envelope>";

            using (var client = new HttpClient())
            {
                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                var response = await client.PostAsync("http://10.1.200.153:7003/FCUBSCustomerService/FCUBSCustomerService", content);
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { error = "Error: Unable to fetch data" });
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var xmlDoc = XDocument.Parse(responseString);
                XNamespace ns = "http://fcubs.ofss.com/service/FCUBSCustomerService";
                var customerElement = xmlDoc.Descendants(ns + "Customer-Full").FirstOrDefault();

                if (customerElement == null)
                {
                    return Json(new { error = "Customer not found" });
                }

                var customerDetails = new
                {
                    Name = customerElement.Element(ns + "NAME")?.Value,
                    MobileNumber = customerElement.Element(ns + "MOBNUM")?.Value,
                    CustomerType = customerElement.Element(ns + "CTYPE")?.Value,
                    AddressLine1 = customerElement.Element(ns + "ADDRLN1")?.Value,
                    StaffStatus = customerElement.Element(ns + "SSTAFF")?.Value,
                    BranchNumber = customerElement.Element(ns + "LBRN")?.Value
                };

                return Json(customerDetails);
            }
        }


        //        public async Task<JsonResult> GetCustomerDetails2(string custNo)
        //        {
        //            var soapRequest = $@"
        //<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:fcub='http://fcubs.ofss.com/service/FCUBSCustomerService'>
        //   <soapenv:Header/>
        //   <soapenv:Body>
        //      <fcub:QUERYCUSTOMER_IOFS_REQ>
        //         <fcub:FCUBS_HEADER>
        //            <fcub:SOURCE>TLB</fcub:SOURCE>
        //            <fcub:UBSCOMP>FCUBS</fcub:UBSCOMP>
        //            <fcub:USERID>KACHADP</fcub:USERID>
        //            <fcub:BRANCH>000</fcub:BRANCH>
        //            <fcub:SERVICE>FCUBSCustomerService</fcub:SERVICE>
        //            <fcub:OPERATION>QueryCustomer</fcub:OPERATION>
        //            <fcub:ACTION>VIEW</fcub:ACTION>
        //         </fcub:FCUBS_HEADER>
        //         <fcub:FCUBS_BODY>
        //            <fcub:Customer-IO>
        //               <fcub:CUSTNO>{custNo}</fcub:CUSTNO>
        //            </fcub:Customer-IO>
        //         </fcub:FCUBS_BODY>
        //      </fcub:QUERYCUSTOMER_IOFS_REQ>
        //   </soapenv:Body>
        //</soapenv:Envelope>";

        //            using (var client = new HttpClient())
        //            {
        //                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
        //                var response = await client.PostAsync("http://10.1.200.153:7003/FCUBSCustomerService/FCUBSCustomerService", content);
        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    return Json(new { name = "Error: Unable to fetch data" });
        //                }

        //                var responseString = await response.Content.ReadAsStringAsync();

        //                var xmlDoc = XDocument.Parse(responseString);
        //                XNamespace ns = "http://fcubs.ofss.com/service/FCUBSCustomerService";
        //                var customerName = xmlDoc.Descendants(ns + "Customer-Full").FirstOrDefault()?.Element(ns + "NAME")?.Value ?? "Name not found";

        //                return Json(new { name = customerName });

        //            }
        //        }
        ///////////////////////////////////////////////////////////////////////
        ///
        public async Task<ViewResult> GetCustomerDetails(string custNo)
        {
            var soapRequest = $@"
                        <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:fcub='http://fcubs.ofss.com/service/FCUBSCustomerService'>
                           <soapenv:Header/>
                           <soapenv:Body>
                              <fcub:QUERYCUSTOMER_IOFS_REQ>
                                 <fcub:FCUBS_HEADER>
                                    <fcub:SOURCE>TLB</fcub:SOURCE>
                                    <fcub:UBSCOMP>FCUBS</fcub:UBSCOMP>
                                    <fcub:USERID>KACHADP</fcub:USERID>
                                    <fcub:BRANCH>000</fcub:BRANCH>
                                    <fcub:SERVICE>FCUBSCustomerService</fcub:SERVICE>
                                    <fcub:OPERATION>QueryCustomer</fcub:OPERATION>
                                    <fcub:ACTION>VIEW</fcub:ACTION>
                                 </fcub:FCUBS_HEADER>
                                 <fcub:FCUBS_BODY>
                                    <fcub:Customer-IO>
                                       <fcub:CUSTNO>{custNo}</fcub:CUSTNO>
                                    </fcub:Customer-IO>
                                 </fcub:FCUBS_BODY>
                              </fcub:QUERYCUSTOMER_IOFS_REQ>
                           </soapenv:Body>
                        </soapenv:Envelope>";

            using (var client = new HttpClient())
            {
                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                var response = await client.PostAsync("http://10.1.200.153:7003/FCUBSCustomerService/FCUBSCustomerService", content);
                var responseString = await response.Content.ReadAsStringAsync();

                var xmlDoc = XDocument.Parse(responseString);
                XNamespace ns = "http://fcubs.ofss.com/service/FCUBSCustomerService";
                ViewBag.Customer = xmlDoc.Descendants(ns + "Customer-Full").FirstOrDefault()?.Element(ns + "NAME")?.Value ?? "Not found";
            }

            return View("CustomerDetails"); // Render view with the response
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
