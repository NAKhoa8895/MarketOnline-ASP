using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MarketOnline.Models;
using PagedList;
using Microsoft.AspNet.Identity;

namespace MarketOnline.Controllers
{
    public class ItemsController : Controller
    {
        private MarketOnlineContext db = new MarketOnlineContext();

        // GET: Items
        public ActionResult Index(string currentGroup, string currentCity, string group, string city, string searchString, string currentstring, string sortOrder, int? page)
        {


            ViewBag.Group = new SelectList(db.Group.ToList(), "Id", "Name");
            ViewBag.City = new SelectList(db.City.ToList(), "Id", "Name");
            if (group != null)
            {
                page = 1;
            }
            else
            {
                group = currentGroup;
            }
            ViewBag.currentGroup = group;
            if (city != null)
            {
                page = 1;
            }
            else
            {
                city = currentCity;
            }
            ViewBag.currentCity = city;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentstring;
            }
            ViewBag.CurrentFilter = searchString;

            var items = db.Items.Include(i => i.AspNetUsers).Include(i => i.City).Include(i => i.Group);

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString) || s.Name.Contains(searchString));

            }
            if (!string.IsNullOrEmpty(group))
            {
                int getGroup = int.Parse(group);
                items = items.Where(s => s.Group.Id == getGroup);

            }
            if (!string.IsNullOrEmpty(city))
            {
                int getCity = int.Parse(city);
                items = items.Where(s => s.City.Id == getCity);
            }
            if (string.IsNullOrEmpty(sortOrder))
            {
                items = items.OrderBy(s => s.Id);
            }
            else

            if (sortOrder.CompareTo("dateFirst") == 0)
            {
                items = items.OrderBy(s => s.Id);
            }
            else if (sortOrder.CompareTo("priceFirst") == 0)
            {
                items = items.OrderBy(s => s.Price);
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        [Authorize]
        // GET: Items/Create
        public ActionResult Create()
        {

            ViewBag.IDUser = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.IDCity = new SelectList(db.City, "Id", "Name");
            ViewBag.IDGroup = new SelectList(db.Group, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,Adress,PhoneContact,image,IDUser,IDGroup,IDCity")] Items items)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(items);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUser = new SelectList(db.AspNetUsers, "Id", "Email", items.IDUser);
            ViewBag.IDCity = new SelectList(db.City, "Id", "Name", items.IDCity);
            ViewBag.IDGroup = new SelectList(db.Group, "Id", "Name", items.IDGroup);
            return View(items);
        }

        [Authorize]
        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Items items = db.Items.Find(id);
           
            if (items == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserId().CompareTo(items.AspNetUsers.Id) != 0)
            {
                return PartialView("~/Views/Shared/WrongIdEditor.cshtml");
            }

            ViewBag.IDUser = new SelectList(db.AspNetUsers, "Id", "Email", items.IDUser);
            ViewBag.IDCity = new SelectList(db.City, "Id", "Name", items.IDCity);
            ViewBag.IDGroup = new SelectList(db.Group, "Id", "Name", items.IDGroup);
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,Adress,PhoneContact,image,IDUser,IDGroup,IDCity")] Items items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUser = new SelectList(db.AspNetUsers, "Id", "Email", items.IDUser);
            ViewBag.IDCity = new SelectList(db.City, "Id", "Name", items.IDCity);
            ViewBag.IDGroup = new SelectList(db.Group, "Id", "Name", items.IDGroup);
            return View(items);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Items items = db.Items.Find(id);
            db.Items.Remove(items);
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
