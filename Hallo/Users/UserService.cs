using HalloDal.Models;
using HalloDal.Models.Users;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace Hallo.Users {
    public class UserService {

        readonly HalloContext context;
        readonly int localChurchId;
        readonly string churchname;
        readonly DataTable pmoData;

        public UserService(HalloContext context, int localChurchId, string churchname = "", DataTable dt = null) {
            this.context = context;
            this.localChurchId = localChurchId;
            this.churchname = churchname;
            this.pmoData = dt;
        }

        public void LogUserSession(HttpSessionStateBase session,
            object pmoId, object username, object firstname, object lastname, object gender,
            object country, object churchId, object churhName, object homeChurchId, object homeChurchName
        ) {
            LogEntry entry = new LogEntry() {
                PmoId = int.Parse(pmoId.ToString()),
                UserName = username.ToString(),
                FirstName = firstname.ToString(),
                LastName = lastname.ToString(),
                Gender = gender.ToString() == "0" ? "M" : "F",
                Country = country.ToString(),
                ChurchId = int.Parse(churchId.ToString()),
                ChurchName = churhName.ToString(),
                HomeChurchId = int.Parse(homeChurchId.ToString()),
                HomeChurchName = homeChurchName.ToString()
            };
            context.LogEntries.Add(entry);

            //User user = context.Users.FirstOrDefault(x => x.UserId == 21782 );
            User user = context.Users.FirstOrDefault(x => x.UserId == entry.PmoId);

            if (user != null) {
                user.NumberOfVisits += 1;
                user.LastVisit = DateTime.Now;
                user.ChurchId = entry.ChurchId;
                user.ChurchName = entry.ChurchName;
                user.HomeChurchId = entry.HomeChurchId;
                user.HomeChurchName = entry.HomeChurchName;
            } else {
                user = new User() {
                    UserId = entry.PmoId,
                    UserName = entry.UserName,
                    Firstname = entry.FirstName,
                    Lastname = entry.LastName,
                    Gender = entry.Gender,
                    Country = entry.Country,
                    ChurchId = entry.ChurchId,
                    ChurchName = entry.ChurchName,
                    HomeChurchId = entry.HomeChurchId,
                    HomeChurchName = entry.HomeChurchName,
                    NumberOfVisits = 1,
                    LastVisit = DateTime.Now,
                    Authorized = (entry.ChurchId == localChurchId)
                };
                context.Users.Add(user);
            }
            context.SaveChanges();

            session["User"] = user;
        }

        public void SyncUserDatabaseWithPmo() {
            context.Users.ToList().ForEach(x => x.IsInPmo = false);
            context.SaveChanges();

            foreach (DataRow pmoPerson in pmoData.Rows) {
                int pmoId = int.Parse(pmoPerson["Person ID"].ToString());
                DateTime birthdate = (DateTime)pmoPerson["Birthdate"];
                String firstname = pmoPerson["Firstname"].ToString() + " " + pmoPerson["Middlename"].ToString();
                String lastname = pmoPerson["Lastname"].ToString();
                String mobilePhone = pmoPerson["Cell Phone"].ToString();
                String email = pmoPerson["E-Mail"].ToString();
                string g1 = pmoPerson["Guardian"].ToString();
                string g2 = pmoPerson["2. Guardian"].ToString();
                int? guardian1 = String.IsNullOrEmpty(g1) ? 0 : int.Parse(g1.Substring(g1.IndexOf('(') + 1).Replace(")", ""));
                int? guardian2 = String.IsNullOrEmpty(g2) ? 0 : int.Parse(g2.Substring(g2.IndexOf('(') + 1).Replace(")", ""));

                var user = context.Users.FirstOrDefault(x => x.UserId == pmoId);
                if (user == null) {
                    context.Users.Add(new HalloDal.Models.Users.User() {
                        UserId = pmoId,
                        Birthday = birthdate,
                        Firstname = firstname,
                        Lastname = lastname,
                        MobilPhone = mobilePhone,
                        Email = email,
                        ChurchId = localChurchId,
                        ChurchName = churchname,
                        Guardian1 = guardian1 == 0 ? null : guardian1,
                        Guardian2 = guardian2 == 0 ? null : guardian2,
                        Authorized = true
                    });
                } else {
                    user.Birthday = birthdate;
                    user.Firstname = firstname;
                    user.Lastname = lastname;
                    user.MobilPhone = mobilePhone;
                    user.Email = email;
                    user.ChurchId = localChurchId;
                    user.ChurchName = churchname;
                    user.Guardian1 = guardian1 == 0 ? null : guardian1;
                    user.Guardian2 = guardian2 == 0 ? null : guardian2;
                    user.Authorized = true;
                }
                context.SaveChanges();
            }
        }
    }
}