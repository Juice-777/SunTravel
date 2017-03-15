using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SunTravel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SunTravel.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin/Users
        public ActionResult Index()
        {
            var allusers = context.Users.ToList();

            //Search id role
            var roleUser = context.Roles.Where(x => x.Name == "User").FirstOrDefault();
            var roleManager = context.Roles.Where(x => x.Name == "Manager").FirstOrDefault();
            var roleAdmin = context.Roles.Where(x => x.Name == "Admin").FirstOrDefault();

            var users = allusers.Where(x => x.Roles.Select(role => role.RoleId).Contains(roleUser.Id)).ToList();
            var userVM = users.Select(user => new UserViewModel { Username = user.UserName, Roles = string.Join(",", roleUser.Name)}).ToList();

            var admins = allusers.Where(x => x.Roles.Select(role => role.RoleId).Contains(roleAdmin.Id)).ToList();
            var adminsVM = admins.Select(user => new UserViewModel { Username = user.UserName, Roles = string.Join(",", roleAdmin.Name) }).ToList();

            var managers = allusers.Where(x => x.Roles.Select(role => role.RoleId).Contains(roleManager.Id)).ToList();
            var managersVM = admins.Select(user => new UserViewModel { Username = user.UserName, Roles = string.Join(",", roleManager.Name) }).ToList();

            var model = new GroupedUserViewModel { Users = userVM, Admins = adminsVM, Managers = managersVM };
            return View(model);
        }

        //Delete user
        [HttpGet]
        public ActionResult Delete(string UserName)
        {
            var thisUser = context.Users.Where(r => r.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Users.Remove(thisUser);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditRole(string UserName)
        {
            ViewBag.UserName = UserName;

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                                        .GetUserManager<ApplicationUserManager>();
                ApplicationUser user = userManager.FindByName(UserName);
                if (user != null)
                    ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);
            }

            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View();
        }

        //add role to user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(string UserName, string RoleName)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //init role
            var role = new IdentityRole { Name = RoleName };
            // search user
            var user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            // if found user
            if (user != null)
            {
                // add role to user
                userManager.AddToRole(user.Id, role.Name);
                ViewBag.ResultMessage = "Role created successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "Not found user";
            }

            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            ViewBag.UserName = UserName;

            return RedirectToAction("EditRole", new { UserName = UserName });
        }
        //Delete some role from user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }

            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return RedirectToAction("EditRole", new { UserName = UserName });
        }
    }
}
