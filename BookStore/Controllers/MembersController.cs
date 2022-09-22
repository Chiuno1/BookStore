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
    public class MembersController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();


        // GET: Members
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

            var result2 = from m in db.Members
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                result2 = result2.Where(m => m.MemberName.Contains(searchString)).OrderByDescending(m => m.MemberID);
                if (result2.Count() == 0)
                {
                    ViewBag.membererr = "查無此會員";
                }
                var result3 = result2.Where(m => m.MemberName.Contains(searchString)).OrderByDescending(m => m.MemberID);

                int pageSize = 2;
                int pageNumber = (page ?? 1);
                return View(result3.ToPagedList(pageNumber, pageSize));
            }

            var member = db.Members.ToList();

            int pageSize01 = 5;
            int pageNumber01 = (page ?? 1);
            return View(member.ToPagedList(pageNumber01, pageSize01));
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //此ID非PK，故須改以下形式
            Members members = db.Members.Where(m => m.MemberID == id).FirstOrDefault();
            if (members == null)
            {
                return HttpNotFound();
            }
            return PartialView(members); //改為部分檢視，去掉不必顯示的
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberAccount,MemberName,UserPassword,Address,Phone")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(members);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //此ID非PK，故須改以下形式
            Members members = db.Members.Where(m => m.MemberID == id).FirstOrDefault();
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: Members/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberAccount,MemberName,UserPassword,Address,Phone")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.Entry(members).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(members);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Members members = db.Members.Find(id);
            db.Members.Remove(members);
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
