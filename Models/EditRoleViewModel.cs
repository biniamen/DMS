using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareSysProd.Models
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }       

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public IEnumerable<AspNetUser> SystemUsers { get; set; }
        public IEnumerable<AspNetUser> UserInCurrentRole { get; set; }
    }
    public class UserRoleRemoveViewModel
    {
        public string RoleId { get; set; }
        public string Rolename { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }



    }