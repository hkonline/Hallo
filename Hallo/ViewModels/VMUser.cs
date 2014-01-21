using HalloDal.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


namespace Hallo.ViewModels {
    public class VMUser {

        private readonly User user;
        private readonly UserGroup group = null;

        public VMUser() {
            user = new User();
        }

        public VMUser(User user) {
            this.user = user;
        }

        public VMUser(User user, UserGroup group) {
            this.user = user;
            this.group = group;
        }

        public string CompositeGroupUserId { 
            get {
                if (group != null) 
                    return group.UserGroupId + "_" + user.UserId;
                return user.UserId.ToString();
            }
        }

        public static int GetUserIdPart(string compositeKey) { 
            string[] parts = compositeKey.Split('_');
            if (parts.Length > 1) return int.Parse(parts[1]);
            return int.Parse(parts[0]);
        }

        public static int GetGroupIdPart(string compositeKey) {
            string[] parts = compositeKey.Split('_');
            if (parts.Length > 1) return int.Parse(parts[0]);
            return -1;
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