using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Education.Models;

namespace Education.Controllers
{
    public class PaperController : BaseController
    {
        // GET: Paper
        public ActionResult Index()
        {
            return View();
        }
    }
}