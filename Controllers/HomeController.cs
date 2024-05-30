using ShareSysProd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareSysProd.Controllers
{

    public class HomeController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();
        public ActionResult Index()
        {
            // total Shareholders
            //ViewBag.Total_Shareholders = db.Shareholders.Where(a => a.share_id != null && a.share_status != "Closed").Count();
            //// total Subscription
            //ViewBag.Subscribed_capital = db.Subscriptions.Where(a => a.sub_share_id != 0).Select(a => a.number_of_share).DefaultIfEmpty().Sum();
            //// total Subscription
            //ViewBag.Paid_capital = db.Transactions.Where(a => a.trans_share_id != 0).Select(a => a.Credit).DefaultIfEmpty().Sum();
            //// total Weighted
            //ViewBag.total_weighted = db.Transactions.Where(a => a.trans_share_id != 0).Select(a => a.Weighted).DefaultIfEmpty().Sum();
            //// Pending Payment
            //ViewBag.Pending_Payments = db.Transactions.Where(a => a.trans_status != "Pending").Count();
            //// Rejected Payment
            //ViewBag.Rejected_Payments = db.Transactions.Where(a => a.trans_status != "Rejected").Count();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



  
    }
}