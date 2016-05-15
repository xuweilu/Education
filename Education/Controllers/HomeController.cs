using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Education.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsStudent())
                {
                     return RedirectToAction("List", "Sheets");

                }
                else if (IsTeacher())
                {
                    return RedirectToAction("List", "Paper");
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "课程平台是本人使用ASP .NET MVC平台编写的毕业设计";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "我的联系方式";

            return View();
        }
    }
}