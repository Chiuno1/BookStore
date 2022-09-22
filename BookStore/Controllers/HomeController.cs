using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        BookStoreEntities db = new BookStoreEntities();

        public ActionResult Index()
        {          
            return View();
        }

        public ActionResult ProductList()
        {
            //var products = db.Products.Where(p => p.UnitsInStock != 0).ToList();
            return View();
        }

        //PartialView 可共用子view無layout
        [ChildActionOnly]
        public ActionResult _ProductList(int itemCount = 0)
        {
            List<Products> products;

            if (itemCount == 0)
                products = db.Products.Where(p => p.UnitsInStock != 0).OrderByDescending(p => p.ProductID).ToList();
            else
                products = db.Products.Where(p => p.UnitsInStock != 0).OrderByDescending(p => p.ProductID).Take(itemCount).ToList();
            return View(products);
        }

        [ChildActionOnly]
        public ActionResult _ProductPhotoShow()
        {
            var products = db.Products.OrderByDescending(p => p.ProductID).Take(5).ToList();

            return View(products);
        }

        public ActionResult MyCart()
        {
            return View();
        }

        public ActionResult ProductDetail(string id)
        {
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }


        //test
        public ActionResult ProductSelect(string Keyword)
        {
            if (Keyword != null)
            {
                var select =
                    (from p in db.Products where p.ProductName.Contains(Keyword) select p).ToList();
                if (select != null)
                {
                    List<Products> ProductSelect = select;
                    return View(ProductSelect);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

    }
}