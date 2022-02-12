using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    public class RegistrationController : Controller
    {
        private StudentsEntities db = new StudentsEntities();

        // GET: Registration
        public ActionResult Index(string searching)
        {
            var registrations = db.registrations.Include(r => r.batch).Include(r => r.course);
            return View(registrations.Where(x => x.FirstName.Contains(searching) || searching == null).ToList());
        }

        // GET: Registration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registration/Create
        public ActionResult Create()
        {
            ViewBag.Batch_id = new SelectList(db.batches, "Id", "Batch1");
            ViewBag.Course_Id = new SelectList(db.courses, "Id", "Course1");
            return View();
        }

        // POST: Registration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,FirstName,LastName,DOB,Email_ID,Course_Id,Batch_id,MobileNo,Address")] registration registration)
        {
            if (ModelState.IsValid)
            {
                db.registrations.Add(registration);
                db.SaveChanges();
                TempData["AlertMessage"] = "Registration Created Successfully...!";
                return RedirectToAction("Index");
            }

            ViewBag.Batch_id = new SelectList(db.batches, "Id", "Batch1", registration.Batch_id);
            ViewBag.Course_Id = new SelectList(db.courses, "Id", "Course1", registration.Course_Id);
            return View(registration);
        }

        // GET: Registration/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.Batch_id = new SelectList(db.batches, "Id", "Batch1", registration.Batch_id);
            ViewBag.Course_Id = new SelectList(db.courses, "Id", "Course1", registration.Course_Id);
            return View(registration);
        }

        // POST: Registration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,DOB,Email_ID,Course_Id,Batch_id,MobileNo,Address")] registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Registration Updated Successfully...!";
                return RedirectToAction("Index");
            }
            ViewBag.Batch_id = new SelectList(db.batches, "Id", "Batch1", registration.Batch_id);
            ViewBag.Course_Id = new SelectList(db.courses, "Id", "Course1", registration.Course_Id);
            return View(registration);
        }

        // GET: Registration/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            registration registration = db.registrations.Find(id);
            db.registrations.Remove(registration);
            db.SaveChanges();
            TempData["AlertMessage"] = "Registration Deleted Successfully...!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
