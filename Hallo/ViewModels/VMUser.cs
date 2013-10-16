using HalloDal.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hallo.ViewModels {
    public class VMUser {

        private readonly User user;

        public VMUser() {
            user = new User();
        }

        public VMUser(User user) {
            this.user = user;
        }

        public int UserId { 
            get { return user.UserId; }
            set { user.UserId = value; }
        }

        public string Firstname {
            get { return user.Firstname; }
        }

        public string Lastname {
            get { return user.Lastname; }
        }

        public int BirthYear {
            get {
                if (user.Birthday == null) return 0;
                return ((DateTime)user.Birthday).Year;
            }
        }

        public string Label {
            get { return user.Firstname + " " + user.Lastname + " " + BirthYear; }
        }
    }
}