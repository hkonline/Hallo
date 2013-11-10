using Hallo.Core.Users;
using Hallo.Infrastructure;
using Hallo.ViewModels;
using HalloDal.Models;
using HalloDal.Models.Users;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class MailController : HalloController {

        private HalloContext db = new HalloContext();

        public ActionResult Index() {
            return View(new MailViewModel());
        }

        [HttpPost]
        public ActionResult Index(MailViewModel m) {
            List<User> users = new UserGroupService(db).GetUserGroupUsers(m.UserGroupId);

            if (m.SendMail) {
                MailHelper.SendMail(m.Text, m.Headline, users, HalloUser, m.CurrentUserIsSender);
            }
            if (m.SendSms) {
                MailHelper.SendSms(m.Text, users, m.CurrentUserIsSender ? HalloUser.MobilPhone : null);
            }

            return View(new MailViewModel {
                EmailList = new UserGroupService(db).GetUserGroupUsers(m.UserGroupId)
            });
        }

        public JsonResult GetUserGroups() {
            var list = db.UserGroups.Select(o => new { o.GroupName, o.Sql, o.UserGroupId }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}
