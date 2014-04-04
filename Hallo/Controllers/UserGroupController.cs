using Hallo.Core.Users;
using Hallo.ViewModels;
using HalloDal.Models.Content;
using HalloDal.Models.Users;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class UserGroupController : HalloController {

        public ActionResult Index() {
            return View(db.UserGroups.Where(x => x.GroupType == GroupType.SmsGroup).OrderBy(x => x.GroupName).ToList());
        }

        public ActionResult ACIndex() {
            return View(db.UserGroups.Where(x => x.GroupType == GroupType.ACTeam).OrderBy(x => x.GroupName).ToList());
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
            UserGroup newGroup = new UserGroup();
            newGroup.GroupType = GroupType.SmsGroup;

            db.UserGroups.Add(newGroup);
            db.SaveChanges();

            return RedirectToAction("Edit/" + newGroup.UserGroupId);
        }

        public ActionResult CreateAC() {
            UserGroup newGroup = new UserGroup();
            newGroup.GroupType = GroupType.ACTeam;

            db.UserGroups.Add(newGroup);
            db.SaveChanges();

            return RedirectToAction("Edit/" + newGroup.UserGroupId);
        }

        public ActionResult Edit(int id = 0) {
            UserGroup usergroup = db.UserGroups.Include("GroupImage").FirstOrDefault(x=>x.UserGroupId == id);
            if (usergroup == null) {
                return HttpNotFound();
            }
            return View(new UserGroupViewModel(usergroup));
        }

        [HttpPost]
        public JsonResult SaveGroupNameAndSql(int groupId, string groupName, string sql) {
            UserGroup group = db.UserGroups.Find(groupId);
            group.GroupName = groupName;
            group.Sql = sql;
            db.SaveChanges();        
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserGroupViewModel usergroup) {
            UserGroup group = db.UserGroups.Find(usergroup.UserGroupId);

            if (ModelState.IsValid) {
                group.GroupName = usergroup.GroupName;
                group.Sql = usergroup.Sql;

                //db.Entry(usergroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction(group.GroupType == GroupType.SmsGroup ? "Index" : "ACIndex");
            }

            return View(new UserGroupViewModel(group));
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
            return RedirectToAction(usergroup.GroupType == GroupType.SmsGroup ? "Index" : "ACIndex");
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult GetUsers([DataSourceRequest] DataSourceRequest request, int groupId) {
            List<VMUser> list = new List<VMUser>();
            db.UserGroups.Find(groupId).Users.Each(x => list.Add(new VMUser(x)));
            return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveUser() {
            int groupId = int.Parse(Request["GroupId"]);
            int userId = int.Parse(Request["UserId"]);

            UserGroup usergroup = db.UserGroups.Find(groupId);
            User user = usergroup.Users.FirstOrDefault(x => x.UserId == userId);
            usergroup.Users.Remove(user);
            db.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult RemoveAdmin() {
            int groupId = int.Parse(Request["GroupId"]);
            int userId = int.Parse(Request["UserId"]);

            UserGroup usergroup = db.UserGroups.Find(groupId);
            User user = usergroup.Administrators.FirstOrDefault(x => x.UserId == userId);
            usergroup.Administrators.Remove(user);
            db.SaveChanges();

            return Json(new { success = true });
        }


        public ActionResult GetAdministrators([DataSourceRequest] DataSourceRequest request, int groupId) {
            List<VMUser> list = new List<VMUser>();
            db.UserGroups.Find(groupId).Administrators.Each(x => list.Add(new VMUser(x)));
            return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddUser(int userId, int groupId) {
            UserGroup usergroup = db.UserGroups.Find(groupId);
            User user = db.Users.Find(userId);

            if (!usergroup.Users.Any(x => x.UserId == userId))
                usergroup.Users.Add(user);

            db.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult AddAdmin(int userId, int groupId) {
            UserGroup usergroup = db.UserGroups.Find(groupId);
            User user = db.Users.Find(userId);

            if (!usergroup.Administrators.Any(x => x.UserId == userId))
                usergroup.Administrators.Add(user);

            db.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult SaveGroupImage(int id, HttpPostedFileBase imageFile) {
            UserGroup group = db.UserGroups.Find(id);
            group.GroupImage = new Image();
            db.Entry(group).State = EntityState.Modified;
            db.SaveChanges();
            SaveImageToDisk(imageFile, group.GroupImage);
            return RedirectToAction("Edit/" + id);
        }

        [HttpPost]
        public ActionResult DeleteGroupImage(int groupId) {
            UserGroup group = db.UserGroups.Include("GroupImage").FirstOrDefault(x => x.UserGroupId == groupId);
            group.GroupImage = null;
            db.Entry(group).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit/" + groupId);
        }
    }
}