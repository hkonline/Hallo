using Hallo.Core.Users;
using HalloDal.Models;
using HalloDal.Models.Users;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class UserGroupController : Controller {
        private HalloContext db = new HalloContext();

        public ActionResult Index() {
            return View(db.UserGroups.ToList());
        }

        public ActionResult Details(int id = 0) {
            UserGroup usergroup = db.UserGroups.Find(id);

            ViewBag.Users = new UserGroupService(db).GetUserGroupUsers(id);

            if (usergroup == null) {
                return HttpNotFound();
            }

            return View(usergroup);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserGroup usergroup) {
            if (ModelState.IsValid) {
                db.UserGroups.Add(usergroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usergroup);
        }

        public ActionResult Edit(int id = 0) {
            UserGroup usergroup = db.UserGroups.Find(id);
            if (usergroup == null) {
                return HttpNotFound();
            }
            return View(usergroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserGroup usergroup) {
            if (ModelState.IsValid) {
                db.Entry(usergroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usergroup);
        }

        public ActionResult Delete(int id = 0) {
            UserGroup usergroup = db.UserGroups.Find(id);
            if (usergroup == null) {
                return HttpNotFound();
            }
            return View(usergroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            UserGroup usergroup = db.UserGroups.Find(id);
            db.UserGroups.Remove(usergroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}