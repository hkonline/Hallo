using Hallo.Core;
using Hallo.Users;
using HalloDal.Models;
using HalloDal.Models.Content;
using HalloDal.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class HalloController : Controller {

        protected HalloContext db = new HalloContext();

        protected JsonSerializerSettings jsonSettings = new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

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

        protected List<User> Family() {
            List<User> family = new List<User>();

            // Add Family 
            family.AddRange(
                db.Users
                    .Where(x => x.Guardian1 == HalloUser.UserId || x.Guardian2 == HalloUser.UserId)
                    .ToList()
            );
            
            if (family.Count == 0)
                // Add user self
                family.Add(HalloUser);
                        
            return family;
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

        protected void SaveImageToDisk(HttpPostedFileBase file, Image image) {
            ImageHelper helper = new ImageHelper(file.InputStream);

            System.Drawing.Image thumb = helper.GetResizedImage(Constant.ThumbnailWidth);

            MemoryStream ms = new MemoryStream();
            thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            System.IO.File.WriteAllBytes(
                Server.MapPath("~/") +
                ConfigurationManager.AppSettings["ImageDirectoryUrl"].Substring(1) +
                "/thumbnails/img" + image.Id + ".jpg",
                ms.ToArray()
            );

            System.Drawing.Image jpgImage;
            if (thumb.Width > thumb.Height) {
                jpgImage = helper.GetResizedImage(720);
            } else {
                jpgImage = helper.GetResizedImage(480);
            }

            ms = new MemoryStream();
            jpgImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            System.IO.File.WriteAllBytes(
                Server.MapPath("~/") +
                ConfigurationManager.AppSettings["ImageDirectoryUrl"].Substring(1) +
                "/images/img" + image.Id + ".jpg",
                ms.ToArray()
            );

            image.Height = jpgImage.Height;
            image.Width = jpgImage.Width;
            db.SaveChanges();
        }

        /*
        private static string InsertFile(string s) {
            int p1 = s.ToLower().IndexOf("&lt;file");
            if (p1 < 0)
                return s;
            int p2 = s.Substring(p1).IndexOf("&gt;");
            string id = s.Substring(p1 + 8, p2 - 8);
            string s2 = s.Substring(0, p1) + SessionFunctions.getFileURL(id) + s.Substring(p1 + p2 + 4);
            return InsertFile(s2);
        }
        */

    }
}
