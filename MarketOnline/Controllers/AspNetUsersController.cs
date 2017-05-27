using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MarketOnline.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;

namespace MarketOnline.Controllers
{
    public class AspNetUsersController : Controller
    {
        private MarketOnlineContext db = new MarketOnlineContext();

        // GET: AspNetUsers
        public ActionResult Index()
        {
            string Id = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(s => s.Id.CompareTo(Id) == 0);
            return View(user.ToList());
        }
        public ActionResult AddImage()
        {
            AspNetUsers user = new AspNetUsers();
            return View(user);
        }

        [HttpPost]
        public ActionResult AddImage([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Name,Adress,DateOfBirth,Image")] AspNetUsers model, HttpPostedFileBase inputimage)
        {
            if (ModelState.IsValid)
            {
                if (inputimage != null)
                {
                    model.Id = User.Identity.GetUserId();

                    model.Image = new byte[inputimage.ContentLength];
                    inputimage.InputStream.Read(model.Image, 0, inputimage.ContentLength);
                }
                db.Entry(model).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }

            return View(model);

        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Name,Adress,DateOfBirth,Image")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUsers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Name,Adress,DateOfBirth,Image")] AspNetUsers aspNetUsers, HttpPostedFileBase inputimage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (inputimage != null)
                    {
                        aspNetUsers.Image = new byte[inputimage.ContentLength];
                        inputimage.InputStream.Read(aspNetUsers.Image, 0, inputimage.ContentLength);
                    }
                    db.AspNetUsers.Attach(aspNetUsers);
                    db.Entry(aspNetUsers).Property(x => x.Email).IsModified = true;
                    db.Entry(aspNetUsers).Property(x => x.PhoneNumber).IsModified = true;
                    db.Entry(aspNetUsers).Property(x => x.UserName).IsModified = true;
                    db.Entry(aspNetUsers).Property(x => x.Name).IsModified = true;
                    db.Entry(aspNetUsers).Property(x => x.Adress).IsModified = true;
                    db.Entry(aspNetUsers).Property(x => x.DateOfBirth).IsModified = true;
                    db.Entry(aspNetUsers).Property(x => x.Image).IsModified = true;
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
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
