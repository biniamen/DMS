using ShareSysProd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareSysProd.Controllers
{
    // Different Type of method for Easly Get Some Dynamic Value
    public class GeneralModule
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();
        // Getting Current Posting Date
        //public DateTime GetPostingDate()
        //{
        //    var posting = db.System_Posting_Date.Where(a => a.date_id != 0).FirstOrDefault();
        //    var date = posting.posting_date;
        //    return date;
        //}
    }
}