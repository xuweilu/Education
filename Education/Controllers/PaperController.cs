using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Education.Models;
using PagedList;
using Education.Abstract;
using Education.Concrete;

namespace Education.Controllers
{
    public class PaperController : BaseController
    {
        private const int PageSize = 10;
        private IEntityRepository<Paper> repository;
        public PaperController(IEntityRepository<Paper> repository)
        {
            this.repository = repository;
        }

        public ActionResult Index(int page = 1)
        {
            return View(DB.Papers.OrderByDescending(p => p.EditOn).ToPagedList(page, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Paper paper)
        {
            if (ModelState.IsValid)
            {
                repository.Add(paper);
            }
        }
    }
}