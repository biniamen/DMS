using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareSysProd.Models
{
    public class DocumentationViewModel
    {
        public string CIF { get; set; }
        public string ClientName { get; set; }
        public string Mobile { get; set; }
        public string Branch { get; set; }
        public string CustomerType { get; set; }
        public int CategoriesId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string CreatedByUserId { get; set; }
        public string ApprovedByUserId { get; set; }
        public string DocumentReference { get; set; }
        public Dictionary<int, HttpPostedFileBase> DocumentFiles { get; set; } // Ensure this matches your form input
    }
    public class DocumentationDetailsViewModel
    {
        public int DocumentationId { get; set; }
        public string CIF { get; set; }
        public string ClientName { get; set; }
        public string Mobile { get; set; }
        public string Branch { get; set; }
        public string CustomerType { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }

        // Additional properties as needed

        public List<DocumentViewModel> Documents { get; set; }
    }

    public class DocumentViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentTypeName { get; set; }
        public string FileLocation { get; set; }
    }
}