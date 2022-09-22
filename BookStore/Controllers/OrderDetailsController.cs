using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using PagedList;

namespace BookStore.Controllers
{
    public class OrderDetailsController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // GET: OrderDetails
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

            var result2 = from od in db.OrderDetails
                          select od;

            if (!String.IsNullOrEmpty(searchString))
            {
                result2 = result2.Where(od => od.OrderID.Contains(searchString)).OrderByDescending(od => od.OrderID);
                if (result2.Count() == 0)
                {
                    ViewBag.orderdetailerr = "查無此訂單明細";
                }
                var result3 = result2.Where(od => od.OrderID.Contains(searchString)).OrderByDescending(od => od.OrderID);

                int pageSize = 2;
                int pageNumber = (page ?? 1);
                return View(result3.ToPagedList(pageNumber, pageSize));
            }

            var orderdetail = db.OrderDetails.ToList();

            int pageSize01 = 5;
            int pageNumber01 = (page ?? 1);
            return View(orderdetail.ToPagedList(pageNumber01, pageSize01));
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "MemberAccount");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: OrderDetails/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,ProductID,Quantity,SellingPrice,Subtotal")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "MemberAccount", orderDetails.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetails.ProductID);
            return View(orderDetails);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //OrderDetails orderDetails = db.OrderDetails.Find(id);
            OrderDetails orderDetails = db.OrderDetails.Where(o => o.OrderID == id).FirstOrDefault();
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "MemberAccount", orderDetails.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetails.ProductID);
            return View(orderDetails);
        }

        // POST: OrderDetails/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ProductID,Quantity,SellingPrice,Subtotal")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "MemberAccount", orderDetails.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetails.ProductID);
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetails);
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
