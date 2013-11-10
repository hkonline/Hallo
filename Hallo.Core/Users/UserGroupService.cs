using HalloDal.Models;
using HalloDal.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hallo.Core.Users {
    public class UserGroupService {

        HalloContext db;

        public UserGroupService(HalloContext context) {
            db = context;
        }

        public List<User> GetUserGroupUsers(int groupId) {
            UserGroup group = db.UserGroups.FirstOrDefault(x => x.UserGroupId == groupId);

            if (group == null) return new List<User>();

            if (!String.IsNullOrEmpty(group.Sql)) {
                List<int> list = db.GetList<int>(group.Sql);
                return db.Users.Where(x => list.Contains(x.UserId)).ToList();
            } else {
                return group.Users.ToList();
            } 
        }

    }
}
