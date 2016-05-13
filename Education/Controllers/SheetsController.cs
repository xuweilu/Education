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
    public class SheetsController : BaseController
    {
        // GET: Sheets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sheets/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Score,AnswerOn,StudentId,ExamId")] Sheet sheet)
        {
            if (ModelState.IsValid)
            {
                sheet.Id = Guid.NewGuid();
                DB.Sheets.Add(sheet);
                await DB.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sheet);
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
