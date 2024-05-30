using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShareSysProd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareSysProd.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private RiskEventDBEntities db = new RiskEventDBEntities();

        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin       
        public ActionResult GetAllUser()
        {
            var AllUser = context.Users.Where(a => a.Id != null);
            return View(AllUser.ToList());
        }

        // Getting User With Role
        public ActionResult UsersWithRoles()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      FullName = user.FullName,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new Users_in_Role_ViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Fullname = p.FullName,
                                      Role = string.Join(",", p.RoleNames)
                                  });


            return View(usersWithRoles);
        }

        public ActionResult UsersWithRoles2()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      FullName = user.FullName,
                                      Role_ID = (from userRole in user.Roles
                                                 join role in context.Roles on userRole.RoleId
                                                 equals role.Id
                                                 select role.Id).ToList(),
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new Users_in_Role_ViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Fullname = p.FullName,
                                      RoleID = p.Role_ID.ToString(),
                                      Role = string.Join(",", p.RoleNames)
                                  });


            return View(usersWithRoles);
        }

        public ActionResult CreateUser()
        {
            ViewBag.branches = (from a in db.Branches_and_Department orderby a.Branch_department_name select a).ToList();
            return View();
        }
        public ActionResult CreateBranches_Users()
        {
            ViewBag.branches = (from a in db.Branches_and_Department select a).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(FormCollection form)
        {
            //var Users_Count = (db.AspNetUsers.Where(a => a.FullName == form["txtfullname"]).Select(a => a.FullName).DefaultIfEmpty()).Count();
            //if(Users_Count == 2)
            //{
            //    TempData["message"] = "Unable to Create User !";
            //    return RedirectToAction("UsersWithRoles");
            //}
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            string UserName = form["txtEmail"];
            string FullName = form["txtfullname"];
            string email = form["txtEmail"];
            string pwd = form["txtPassword"];
            var branch_name = Convert.ToString(form["txtfullname"]);
            var username = form["txtEmail"];
            var User_Information = (context.Users.Where(a => a.FullName == branch_name && a.UserName == username)).FirstOrDefault();

            if (User_Information != null)
            {
                TempData["message"] = "User Already Exist!";
                return RedirectToAction("UsersWithRoles");
            }
            //create default user
            var user = new ApplicationUser();
            user.UserName = UserName;
            user.Email = email;
            user.FullName = FullName;
            var newuser = userManager.Create(user, pwd);
            TempData["message"] = "User Successfully Created !";
            return RedirectToAction("UsersWithRoles");            
        }

        [HttpPost]
        public ActionResult CreateBranchUser(FormCollection form)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            string UserName = form["txtfullname"];
            string FullName = form["txtfullname"];
            string email = form["txtfullname"];
            string pwd = form["txtPassword"];
            //create default user
            var user = new ApplicationUser();
            user.UserName = UserName;
            user.Email = email;
            user.FullName = FullName;
            var newuser = userManager.Create(user, pwd);
            TempData["message"] = "User Successfully Created !";
            return RedirectToAction("UsersWithRoles");
        }
        public ActionResult CreateRole()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult NewRole(FormCollection form)
        {
            string rolename = form["RoleName"];
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists(rolename))
            {
                //create super admin role
                var role = new IdentityRole(rolename);
                roleManager.Create(role);
            }
            //return View("Index");
            return RedirectToAction("Index", "AspNetRoles");
        }

        public ActionResult AssignRole()
        {
            ViewBag.Roles = context.Roles.Select(r => new SelectListItem { Value = r.Name.ToString(), Text = r.Name.ToString() }).ToList();
            ViewBag.Users = context.Users.Select(u => new SelectListItem { Value = u.UserName.ToString(), Text = u.FullName.ToString()+ " || " + u.UserName.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AssignRole(FormCollection form)
        {
            string usrname = form["UserName"];
            string rolname = form["RoleName"];
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(usrname, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.AddToRole(user.Id, rolname);      
            TempData["UserExist"] = usrname +" Successfully Added to " + rolname +" Role";
            return RedirectToAction("AssignRole");

        }

        public ActionResult UserAndTheirRole()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new Users_in_Role_ViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  });


            return View(usersWithRoles);
        }
        // Removing Role from User

        //public ActionResult ReomveRoleFromUser(string UserID)
        //{
        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        //    var userRole = new IdentityRole();          
        //    var users = context.Users.Where(x => x.Id == UserID).SingleOrDefault();
        //    var UserInrole = roleManager.FindById(UserID)



        //    IdentityUserRole userRole = new IdentityUserRole();
        //    userRole.RoleId = role.Id;
        //    userRole.UserId = user.Id;


        //    role.Users.Remove(userRole);
        //    RoleManager.Update(role); // nothing happens
        //}
    }
}