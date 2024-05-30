using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareSysProd.Models
{
    public class RiskViewModel
    {
        public int master_book_id { get; set; }
        public string branch_name { get; set; }
        public Nullable<System.DateTime> date_of_operation_commenced { get; set; }
        public System.DateTime date_of_risk_event_occured { get; set; }
        public System.DateTime date_of_risk_event_identified { get; set; }
        public string RIF { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.MultilineText)]
        public string risk_area { get; set; }
        public int Risk_Event_subcategory_id { get; set; }
        public string Risk_Description { get; set; }
        public int level_of_risk_frequency { get; set; }
        public int level_of_risk_impact { get; set; }
        public string risk_rating { get; set; }
        public string root_cause { get; set; }

        [Required]
        public string Existing_mitigation_control { get; set; }
        public string Proposed_mitigation_control { get; set; }
        public string Key_Risk_Indicators { get; set; }
        public string Status_resolved { get; set; }
        public Nullable<System.DateTime> date_of_resolved { get; set; }
        public string Status_Follow_Up_by_risk_owner { get; set; }
        public string Effort_exerted_to_resolve_risk { get; set; }
        public string Transferred_to_legal_department { get; set; }
        public Nullable<System.DateTime> transferred_to_legal_date { get; set; }
        public string Transferred_Decipline_Committe { get; set; }
        public Nullable<System.DateTime> Transferred_to_decipline_date { get; set; }
        public string Is_Existing { get; set; }
        public string Is_there_Cost_of_proposed_action { get; set; }
        public string created_by { get; set; }
        public string edited_by { get; set; }
        public string Risk_Owner { get; set; }
        public string Risk_Owner_2 { get; set; }
        public string Risk_Owner_3 { get; set; }
        public string Risk_Owner_4 { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public string approved_by { get; set; }
        public Nullable<System.DateTime> approved_date { get; set; }
        public string Status { get; set; }
        public string Supporting_document { get; set; }
        public string Cost_of_action { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
    }
}