using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BookStore.Models;
using PagedList;

namespace BookStore.Controllers
{
    public class SuppliersController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Suppliers
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var result2 = from s in db.Suppliers
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                result2 = result2.Where(s => s.SupplierName.Contains(searchString)).OrderByDescending(s => s.SupplierID);
                if (result2.Count() == 0)
                {
                    ViewBag.suppliererr = "查無此供應商";
                }
                var result3 = result2.Where(s => s.SupplierName.Contains(searchString)).OrderByDescending(s => s.SupplierID);

                int pageSize = 2;
                int pageNumber = (page ?? 1);
                return View(result3.ToPagedList(pageNumber, pageSize));
            }

            var supplier = db.Suppliers.ToList();

            int pageSize01 = 5;
            int pageNumber01 = (page ?? 1);
            return View(supplier.ToPagedList(pageNumber01, pageSize01));
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Suppliers suppliers = db.Suppliers.Find(id);
            Suppliers suppliers = db.Suppliers.Where(s => s.SupplierID == id).FirstOrDefault();
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierID,SupplierAccount,SupplierName,ContactName,SupplierPassword")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(suppliers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(suppliers);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Suppliers suppliers = db.Suppliers.Find(id);
            Suppliers suppliers = db.Suppliers.Where(m => m.SupplierID == id).FirstOrDefault();
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Suppliers/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierID,SupplierAccount,SupplierName,ContactName,SupplierPassword")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suppliers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Suppliers suppliers = db.Suppliers.Find(id);
            db.Suppliers.Remove(suppliers);
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
