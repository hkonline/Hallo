using Hallo.ViewModels;
using HalloDal.Models.Users;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class RoleController : HalloController {

        public ActionResult Index() {
            Authorize();

            return View(db.Roles.ToList());
        }

        public ActionResult Details(int id = 0) {
            Authorize();
            
            Role role = db.Roles.Find(id);
            if (role == null) {
                return HttpNotFound();
            }
            return View(role);
        }

        public ActionResult Create() {
            Authorize();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role) {
            Authorize();
            
            if (ModelState.IsValid) {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        public ActionResult Edit(int id = 0) {
            Authorize();
            
            Role role = db.Roles.Find(id);
            if (role == null) {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role) {
            Authorize();
            
            if (ModelState.IsValid) {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        public ActionResult Delete(int id = 0) {
            Authorize();
            
            Role role = db.Roles.Find(id);
            if (role == null) {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Authorize();
            
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult GetUserItems() {
            List<VMUser> list = new List<VMUser>();
            DateTime ageLimit = DateTime.Now.AddYears(-14);

            var userList = db.Users
                .Where(x => x.Birthday < ageLimit)
                .Where(x => x.Authorized == true)
                .OrderBy(x => x.Firstname)
                .ThenBy(x => x.Lastname).ToList();

            userList.ForEach(x => list.Add(new VMUser(x)));

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserItemsLimit6() {
            List<VMUser> list = new List<VMUser>();
            DateTime ageLimit = DateTime.Now.AddYears(-6);

            var userList = db.Users
                .Where(x => x.Birthday < ageLimit)
                .Where(x => x.Authorized == true)
                .OrderBy(x => x.Firstname)
                .ThenBy(x => x.Lastname).ToList();

            userList.ForEach(x => list.Add(new VMUser(x)));

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleItems() {
            var list = new List<KeyValuePair<int, string>>();
            foreach (Role r in db.Roles) list.Add(new KeyValuePair<int, string>(r.RoleId, r.RoleName));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public IList<VMUser> GetRoleUserList(int roleId) {
            Role role = db.Roles.Include(x => x.Users).FirstOrDefault(x => x.RoleId == roleId);

            List<VMUser> list = new List<VMUser>();
            if (role.Users != null)
                role.Users.ToList().ForEach(x => list.Add(new VMUser(x)));

            return list;
        }

        public JsonResult GetRoleUsers([DataSourceRequest] DataSourceRequest request, int roleId) {
            DataSourceResult res = new DataSourceResult { Data = GetRoleUserList(roleId) };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Admin() {
            Authorize("Editor");

            var list = new List<KeyValuePair<int, string>>();
            foreach (Role r in db.Roles) list.Add(new KeyValuePair<int, string>(r.RoleId, r.RoleName));
            ViewBag.RoleList = list;

            DateTime ageLimit = DateTime.Now.AddYears(-14);
            ViewBag.UserList = db.Users
                .Where(x => x.Birthday < ageLimit)
                .OrderBy(x => x.Firstname)
                .ThenBy(x => x.Lastname).ToList();

            ViewBag.RoleUserList = GetRoleUserList(1);

            return View();
        }

        [HttpPost]
        public JsonResult DeleteRoleUser([DataSourceRequest] DataSourceRequest request, VMUser user, int roleId) {
            Authorize("Editor");            
            if (user != null && user.UserId > 0) {
                db.Roles.FirstOrDefault(r => r.RoleId == roleId).Users.Remove(db.Users.FirstOrDefault(x => x.UserId == user.UserId));
                db.SaveChanges();
            }
            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public JsonResult CreateRoleUser(int roleId, int userId) {
            Authorize("Editor");            
            db.Roles.First(x => x.RoleId == roleId).Users.Add(db.GetUserById(userId));
            db.SaveChanges();
            return Json(null);
        }


    }
}