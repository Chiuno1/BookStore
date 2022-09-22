using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class HomeManagerController : Controller
    {
        BookStoreEntities db = new BookStoreEntities();

        [LoginCheck]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(VMLogin vmLogin)
        {

            var user = db.Administrator.Where(m => m.AdminAccount == vmLogin.Account && m.AdminPassword == vmLogin.Password).FirstOrDefault();

            //會員登入
            var mbuser = db.Members.Where(m => m.MemberAccount == vmLogin.Account && m.UserPassword == vmLogin.Password).FirstOrDefault();

            //供應商登入
            var supuser = db.Suppliers.Where(m => m.SupplierAccount == vmLogin.Account && m.SupplierPassword == vmLogin.Password).FirstOrDefault();
            
            if (user == null && mbuser == null && supuser == null)
            {
                ViewBag.ErrMsg = "帳號或密碼錯誤!";
                return View(vmLogin);
            }

            if (user != null)
            {
                Session["username"] = user;
                
                return RedirectToAction("Index","HomeManager");
            }

            else if (mbuser != null)
            {
                Session["username"] = mbuser;
                Session["msname"] = mbuser.MemberName;
                return RedirectToAction("Index", "Home");
            }

            else
            {              
                Session["username"] = supuser;
                Session["msname"] = supuser.ContactName;
                return RedirectToAction("Index", "Home");
            }
           
        }

        //[LoginCheck]
        public ActionResult Logout()
        {
            Session["username"] = null;

            //會員登入
            Session["mbuser"] = null;

            //供應商登入
            Session["supuser"] = null;

            return RedirectToAction("Index", "Home");
        }

    }

}