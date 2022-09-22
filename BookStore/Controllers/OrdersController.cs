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
    public class OrdersController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Orders
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

            var result2 = from o in db.Orders
                          select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                result2 = result2.Where(o => o.OrderID.Contains(searchString)).OrderByDescending(o => o.OrderID);
                if (result2.Count() == 0)
                {
                    ViewBag.ordererr = "查無此訂單";
                }
                var result3 = result2.Where(o => o.OrderID.Contains(searchString)).OrderByDescending(o => o.OrderID);

                int pageSize = 2;
                int pageNumber = (page ?? 1);
                return View(result3.ToPagedList(pageNumber, pageSize));
            }

            var order = db.Orders.ToList();

            int pageSize01 = 5;
            int pageNumber01 = (page ?? 1);
            return View(order.ToPagedList(pageNumber01, pageSize01));
        }

        // GET: Orders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.MemberAccount = new SelectList(db.Members, "MemberAccount", "MemberName");
            ViewBag.PayTypesID = new SelectList(db.PayTypes, "PayTypesID", "PayTypeName");
            return View();
        }

        // POST: Orders/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,MemberAccount,TotalPrice,OrderDate,ShipDate,ShipAddress,PayTypesID")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberAccount = new SelectList(db.Members, "MemberAccount", "MemberName", orders.MemberAccount);
            ViewBag.PayTypesID = new SelectList(db.PayTypes, "PayTypesID", "PayTypeName", orders.PayTypesID);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberAccount = new SelectList(db.Members, "MemberAccount", "MemberName", orders.MemberAccount);
            ViewBag.PayTypesID = new SelectList(db.PayTypes, "PayTypesID", "PayTypeName", orders.PayTypesID);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,MemberAccount,TotalPrice,OrderDate,ShipDate,ShipAddress,PayTypesID")] Orders orders)
        {        
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();                              
                return RedirectToAction("Index");
            }
            ViewBag.MemberAccount = new SelectList(db.Members, "MemberAccount", "MemberName", orders.MemberAccount);
            ViewBag.PayTypesID = new SelectList(db.PayTypes, "PayTypesID", "PayTypeName", orders.PayTypesID);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
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
