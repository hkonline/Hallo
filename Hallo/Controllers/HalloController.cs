using Hallo.Users;
using HalloDal.Models;
using HalloDal.Models.Users;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class HalloController : Controller {

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

        private HalloContext context;
        protected HalloContext Context {
            get {
                if (context == null) context = new HalloContext();

                return context;
            }
        }
    }
}
