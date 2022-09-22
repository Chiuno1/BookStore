using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class MemberOrderController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        [ChildActionOnly]
        public ActionResult _Order()
        {
            ViewBag.PayTypesID = new SelectList(db.PayTypes, "PayTypesID", "PayTypeName");
            
            return View();
        }

        [HttpPost]
        public ActionResult Order(string ShipAddress, int PayTypesID, string data)
        {
            //如果沒登入,導去登入
            if (Session["username"] == null)
                return RedirectToAction("Login", "HomeManager");

            string MemberAccount = ((Members)Session["username"]).MemberAccount; 
            long memberID = ((Members)Session["username"]).MemberID;   

            //如果已登入,就寫入資料庫
            List<SqlParameter> list = new List<SqlParameter>
            {
                new SqlParameter("MemberAccount",MemberAccount),
                new SqlParameter("ShipAddress",ShipAddress),             
                new SqlParameter("PayTypesID",PayTypesID),
                new SqlParameter("MemberID",memberID),
                new SqlParameter("cart",data)
            };
            SetData sd = new SetData();
            sd.executeSqlBySP("addOrders", list);

            TempData["cartFlag"] = "OK";

            return RedirectToAction("MyCart", "Home");

        }
    }
}