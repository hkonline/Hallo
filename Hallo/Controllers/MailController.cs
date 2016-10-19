using Hallo.Core.Users;
using Hallo.Infrastructure;
using Hallo.ViewModels;
using HalloDal.Models;
using HalloDal.Models.Users;
using System.Data.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class MailController : HalloController {

        //private HalloContext db = new HalloContext();

        public ActionResult Index() {
            return View(new MailViewModel());
        }

        [HttpPost]
        public ActionResult Index(MailViewModel m) {
            List<User> users = new UserGroupService(db).GetUserGroupUsers(m.UserGroupId);

            if (m.SendMail) {
                m.EmailList = MailHelper.SendMail(m.Text, m.Headline, users, HalloUser, m.CurrentUserIsSender);
            }
            if (m.SendSms) {
                m.SmsList = MailHelper.SendSms(m.Text, users, m.CurrentUserIsSender ? HalloUser.MobilPhone : null);

                db.SmsLogs.Add(new SmsLog { 
                    NumberOfSms = m.SmsList.Count,
                    Text = m.Text,
                    SendingUser = db.Users.FirstOrDefault(x=>x.UserId == HalloUser.UserId),
                    SmsTime = DateTime.Now
                });

                db.SaveChanges();
            }

            return View(m);
        }

        public JsonResult GetUserGroups() {
            var list = db.UserGroups.Where(g=>g.GroupType==GroupType.SmsGroup).Select(o => new { o.GroupName, o.Sql, o.UserGroupId }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SmsLog() {
            DateTime startdate = DateTime.Now.AddMonths(-3);            
            List<SmsLog> list = db.SmsLogs.Include(x=>x.SendingUser).Where(x => x.SmsTime > startdate).OrderByDescending(x=>x.SmsTime).ToList();            
            return View(list);
        }

    }
}
