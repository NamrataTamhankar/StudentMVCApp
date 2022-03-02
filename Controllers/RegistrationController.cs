using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using StudentApp.Models;


namespace StudentApp.Controllers
{
    public class RegistrationController : Controller
    {
        private StudentsEntities db = new StudentsEntities();

        // GET: Registration
        public ActionResult Index(string searching, string SortOrder, String SortBy)
        {
            ViewBag.SortOrder = SortOrder;
            var registrations = db.registrations.Include(r => r.batch).Include(r => r.course).ToList();

            switch (SortBy)
            {

                case "FirstName":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    registrations = registrations.OrderBy(x => x.FirstName).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    registrations = registrations.OrderByDescending(x => x.FirstName).ToList();
                                    break;
                                }
                            default:
                                {
                                    registrations = registrations.OrderBy(x => x.FirstName).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "Email_ID":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    registrations = registrations.OrderBy(x => x.Email_ID).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    registrations = registrations.OrderByDescending(x => x.Email_ID).ToList();
                                    break;
                                }
                            default:
                                {
                                    registrations = registrations.OrderBy(x => x.Email_ID).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        registrations = registrations.OrderBy(x => x.FirstName).ToList();
                        break;
                    }
            }
            return View(registrations);
        }

        [HttpPost]
        public ActionResult Index(string searching)
        {
            var registrations = db.registrations.ToList();/*Include(r => r.batch).Include(r => r.course) */
            return View(registrations.Where(x => x.FirstName.Contains(searching) || x.Address.Contains(searching) || searching == null).ToList());
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
        [HttpGet]
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
        public ActionResult Create(HttpPostedFileBase ImageFile,[Bind(Include = "id,FirstName,LastName,DOB,Email_ID,Course_Id,Batch_id,MobileNo,Address,Image,ImageFile")] registration registration)
        {
            //string filename = Path.GetFileNameWithoutExtension(registration.ImageFile.FileName);
            //string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            //string extension = Path.GetExtension(registration.ImageFile.FileName);
            //string path = Path.Combine(Server.MapPath("../Image/"), _filename);
            //registration.Image = "../Image/" + _filename;

            //if(extension.ToLower() ==".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            //{
            //    if (ImageFile.ContentLength < 1000000)
            //    {
            //        db.registrations.Add(registration);
            //        if(db.SaveChanges() > 0)
            //        {
            //            ImageFile.SaveAs(path);
            //            ViewBag.msg = "Record Added";
            //            ModelState.Clear();


            //        }

            //    }
            //    else
            //    {
            //        ViewBag.msg = "Size not valid";
            //    }
            //}

            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(registration.ImageFile.FileName);
                string extension = Path.GetExtension(registration.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                registration.Image = "../Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("../Image/"), fileName);
                registration.ImageFile.SaveAs(fileName);
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
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,DOB,Email_ID,Course_Id,Batch_id,MobileNo,Address,Image,ImageFile")] registration registration)
        { 
            if (ModelState.IsValid)
            { 
                if(registration.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(registration.ImageFile.FileName);
                    string extension = Path.GetExtension(registration.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    registration.Image = "../Image/" + fileName; 
                    fileName = Path.Combine(Server.MapPath("../Image/"), fileName);
                    registration.ImageFile.SaveAs(fileName);

                    db.registrations.Add(registration);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Registration Created Successfully...!";
                    return RedirectToAction("Index");
                }
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
