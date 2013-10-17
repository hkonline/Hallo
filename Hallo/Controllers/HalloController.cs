﻿using Hallo.Users;
using HalloDal.Models;
using HalloDal.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class HalloController : Controller {
        protected HalloContext db = new HalloContext();

        protected void InitializeSession() {
            UserService userService = new UserService(
                new HalloContext(),
                int.Parse(System.Configuration.ConfigurationManager.AppSettings["ChurchId"])
            );

            userService.LogUserSession(
                Session,
                Session["SIGNON_Attribute_PMO:PersonId"], Session["SIGNON_Username"],
                Session["SIGNON_FirstName"], Session["SIGNON_LastName"],
                Session["SIGNON_Gender"], Session["SIGNON_Attribute_PMO:Country"],
                Session["SIGNON_Attribute_PMO:ChurchId"], Session["SIGNON_Attribute_PMO:ChurchName"],
                Session["SIGNON_Attribute_PMO:HomeChurchId"], Session["SIGNON_Attribute_PMO:HomeChurchName"]
            );
        }

        protected User HalloUser {
            get {
                if (Session["User"] == null) InitializeSession();
                return Session["User"] as User;
            }
        }

        public static bool IsAuthorized(User user, string role1 = null, string role2 = null, string role3 = null) {
            List<String> currentRoles = new List<string>();
            foreach (Role r in user.Roles) currentRoles.Add(r.RoleName);

            if (user.UserId == 17787) return true; // Is Reuss?
            if (currentRoles.Contains("Webmaster")) return true;
            if (role1 != null && currentRoles.Contains(role1)) return true;
            if (role2 != null && currentRoles.Contains(role2)) return true;
            if (role3 != null && currentRoles.Contains(role3)) return true;

            return false;            
        }

        protected void Authorize(string role1 = null, string role2 = null, string role3 = null) {
            if (IsAuthorized(HalloUser, role1, role2, role3)) return;

            Response.Redirect("/Home/NoAccess");
        }
    }
}
