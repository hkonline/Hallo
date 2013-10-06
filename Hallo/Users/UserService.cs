using HalloDal.Models;
using HalloDal.Models.Users;
using System;
using System.Linq;
using System.Web;

namespace Hallo.Users {
    public class UserService {

        readonly HalloContext context;
        readonly int localChurchId;

        public UserService(HalloContext context, int localChurchId) {
            this.context = context;
            this.localChurchId = localChurchId;
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

    }
}