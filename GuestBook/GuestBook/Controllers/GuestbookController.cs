using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuestBook.Models;

namespace GuestBook.Controllers
{
    public class GuestbookController : Controller
    {
        // GET: Guestbook
        private GuestbookContext _db=new GuestbookContext();
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GuestbookEntry entry)
        {
            if (ModelState.IsValid)
            {
                entry.DateAdded = DateTime.Now;
                _db.Entries.Add(entry);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entry);
        }

        public ActionResult Index()
        {
            var mostRecentEntries =
                (from entry in _db.Entries orderby entry.DateAdded descending select entry).Take(20);
            var model = mostRecentEntries.ToList();
            return View(model);
        }

        public ViewResult Show(int id)
        {
            var entry = _db.Entries.Find(id);
            bool hasPermission = User.Identity.Name == entry.Name;
            ViewBag.HasPermission = hasPermission;
            return View(entry);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entry = _db.Entries.Find(id);
            _db.Entries.Attach(entry);
            _db.Entries.Remove(entry);
            _db.SaveChanges();
             return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}