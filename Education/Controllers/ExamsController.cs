using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Education.Models;

namespace Education.Controllers
{
    public class ExamsController : BaseController
    {
        // GET: Exams
        public async Task<ActionResult> Index()
        {
            return View(await DB.Exams.ToListAsync());
        }

        // GET: Exams/Create
        [HttpGet]
        public async Task<ActionResult> Create(Guid id)
        {
            var Exam = await DB.Exams.FindAsync(id);
            return View(Exam);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, FormCollection collection)
        {
            var exam = await DB.Exams.FirstOrDefaultAsync(e => e.Id == id);
            if (ModelState.IsValid && TryUpdateModel(exam, "", collection.AllKeys))
            {
                await DB.SaveChangesAsync();
                return RedirectToAction("List", "Paper");
            }
            return View(exam);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
