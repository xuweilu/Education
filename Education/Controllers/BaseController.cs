using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Education.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace Education.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationDbContext _db;
        protected ApplicationDbContext DB
        {
            get
            {
                if(_db == null)
                {
                    _db = new ApplicationDbContext();
                    return _db;
                }
                return _db;
            }
        }
        private ApplicationUserManager _appUserManager = null;
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _appUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private ApplicationRoleManager _appRoleManager = null;
        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _appRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected string CurrentUserName
        {
            get
            {
                return User.Identity.Name;
            }
        }
        protected async Task<ApplicationUser> GetUserByNameAsync(string username)
        {
            return await AppUserManager.FindByNameAsync(username);
        }
        protected ApplicationUser GetCurrentUser()
        {
            return DB.Users.FirstOrDefault(u => u.UserName == CurrentUserName);
        }
        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await AppUserManager.FindByNameAsync(CurrentUserName);
        }
        protected bool IsStudent()
        {
            return User.IsInRole(Role.Student);
        }
        protected bool IsAdmin()
        {
            return User.IsInRole(Role.Administrator);
        }
        protected bool IsTeacher()
        {
            return User.IsInRole(Role.Teacher);
        }
    }
}