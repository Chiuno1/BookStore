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
    public class ProductsController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Products
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

            var result2 = from p in db.Products
                          select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                result2 = result2.Where(p => p.ProductName.Contains(searchString)).OrderByDescending(p=>p.ProductID);
                if (result2.Count() == 0)
                {
                    ViewBag.producterr = "查無此商品";
                }
                var result3 = result2.Where(p => p.ProductName.Contains(searchString)).OrderByDescending(p => p.ProductID);

                int pageSize = 2;
                int pageNumber = (page ?? 1);
                return View(result3.ToPagedList(pageNumber, pageSize));
            }
            
            var product = db.Products.ToList();

            int pageSize01 = 5;
            int pageNumber01 = (page ?? 1);
            return View(product.ToPagedList(pageNumber01, pageSize01));
        }

        [LogReporter(recordFlag = false)]
        public string GetImage(string id)
        {
            var photo = db.Products.Find(id);
            if (photo == null)
                return null;

            return photo.ProductImg;
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.SupplierAccount = new SelectList(db.Suppliers, "SupplierAccount", "SupplierName");
            return View();
        }

        // POST: Products/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierAccount,UnitsInStock,UnitPrice,ProductImg,ImageMimeType")] Products products, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    string extensionName = System.IO.Path.GetExtension(photo.FileName);

                    if (extensionName == ".jpg" || extensionName == ".png" || extensionName == ".jpeg")
                    {
                        photo.SaveAs(Server.MapPath("~/ProductImg/" + products.ProductName + extensionName));
                        products.ProductImg = "/ProductImg/"+ products.ProductName + extensionName;
                    }
                }
            }

            if (photo == null)
            {
                ViewBag.ErrMessage = "請上傳商品照片";
                return View(products);
            }

            if (db.Products.Find(products.ProductID) != null)
            {
                ViewBag.ErrMessage2 = "商品編號重複";
                return View(products);
            }

            //products.ImageMimeType = photo.ContentType;
            //products.ProductImg = new byte[photo.ContentLength];
            //photo.InputStream.Read(products.ProductImg, 0, photo.ContentLength);

            //ModelState.Remove("ProductImg");

            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.SupplierAccount = new SelectList(db.Suppliers, "SupplierAccount", "SupplierName", products.SupplierAccount);
            return PartialView(products);           
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SupplierAccount = new SelectList(db.Suppliers, "SupplierAccount", "SupplierName", products.SupplierAccount);
            return View(products);
        }

        // POST: Products/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products, HttpPostedFileBase photo)
        {
            //if (photo != null)
            //{
            //    products.ImageMimeType = photo.ContentType;
            //    products.ProductImg = new byte[photo.ContentLength];
            //    photo.InputStream.Read(new byte[products.ProductImg], 0, photo.ContentLength);
            //}

            //ModelState.Remove("ProductImg");

            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    string extensionName = System.IO.Path.GetExtension(photo.FileName);

                    if (extensionName == ".jpg" || extensionName == ".png" || extensionName== ".jpeg")
                    {
                        photo.SaveAs(Server.MapPath("~/ProductImg/" + products.ProductName + extensionName));
                        products.ProductImg = "/ProductImg/" + products.ProductName + extensionName;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.SupplierAccount = new SelectList(db.Suppliers, "SupplierAccount", "SupplierName", products.SupplierAccount);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
