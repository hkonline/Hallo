using Hallo.ViewModels;
using HalloDal.Models.AC;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HalloDal.Models.Users;

namespace Hallo.Controllers {
    public class AcPlanController : HalloController {

        public ActionResult TeamOverview(bool all) {
            ViewBag.Headline = all ? "Oversigt alle AK-hold" : "AK-hold";

            List<UserGroupViewModel> list = new List<UserGroupViewModel>();
            List<UserGroup> groups = new List<UserGroup>();
            List<User> family = Family();
            List<int> familyIds = new List<int>();

            if (!all) 
                foreach (var u in family) {
                    groups.AddRange(u.UserGroups.Where(g => g.GroupType == GroupType.ACTeam).ToList());
                    groups.AddRange(u.AdminUserGroups.Where(g => g.GroupType == GroupType.ACTeam).ToList());
                }

            if (groups.Count == 0) all = true;

            if (all) 
                groups = db.UserGroups
                    .Include("Users")
                    .Include("Administrators")
                    .Where(x => x.GroupType == GroupType.ACTeam).ToList();
            
            foreach (var u in family) familyIds.Add(u.UserId);
            foreach (UserGroup group in groups) 
                list.Add(new UserGroupViewModel(group) { 
                    NextActivity = GetNextActivity(group.UserGroupId) 
                });

            ViewBag.Family = familyIds;

            return View(list);
        }

        private AcPlanViewModel GetNextActivity(int teamId) {
            AcDate date = db.AcDates.Where(x => x.Date >= DateTime.Today).OrderBy(x => x.Date).FirstOrDefault();
            if (date == null)
                return null;
            AcPlanEntry entry = db.AcPlanEntries.Where(x => x.Date.Date == date.Date && x.TeamId == teamId).FirstOrDefault();
            AcPlanViewModel m = new AcPlanViewModel { Date = date.Date };
            if (entry != null) {
                m.Activity = entry.Activity;
                m.Remember = entry.Remember;
            }
            return m;
        }

        public JsonResult GetAcPlan([DataSourceRequest] DataSourceRequest request, int teamId) {
            List<AcDate> dates = db.AcDates.Where(x => x.Date >= DateTime.Today).OrderBy(x => x.Date).ToList();
            List<AcPlanViewModel> list = new List<AcPlanViewModel>();
            foreach (AcDate date in dates) {
                AcPlanViewModel vm = new AcPlanViewModel {
                    TeamId = teamId,
                    Date = date.Date,
                    DateId = date.Id
                };
                AcPlanEntry entry = db.AcPlanEntries.FirstOrDefault(x => x.TeamId == teamId && x.Date.Date == date.Date);
                if (entry != null) {
                    vm.Activity = entry.Activity;
                    vm.Remember = entry.Remember;
                }
                list.Add(vm);
            }
            return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Plan(int id) {
            UserGroup group = db.UserGroups.Find(id);
            ViewBag.Headline = "AK-Plan for " + group.GroupName;
            ViewBag.IsAdmin = IsAuthorized(HalloUser, "AcAdmin");
            foreach (UserGroup adminGroup in HalloUser.AdminUserGroups) 
                if (adminGroup.UserGroupId == id) ViewBag.IsAdmin = true;
            return View(id);
        }

        public JsonResult UpdatePlan(AcPlanViewModel plan) {
            AcPlanEntry entry = db.AcPlanEntries.FirstOrDefault(x => x.TeamId == plan.TeamId && x.Date.Id == plan.DateId);
            if (entry != null) {
                entry.Activity = plan.Activity;
                entry.Remember = plan.Remember;
            } else {
                entry = new AcPlanEntry {
                    Date = db.AcDates.Find(plan.DateId),
                    TeamId = plan.TeamId,
                    Activity = plan.Activity,
                    Remember = plan.Remember
                };
                db.AcPlanEntries.Add(entry);
            }
            db.SaveChanges();
            return Json(new { success = true });
        }

    }
}