using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HalloDal.Models.Users {
    public class UserGroup {
        public int UserGroupId { get; set; }

        [Display(Name = "GroupName", ResourceType = typeof(Resources.DisplayNames))]
        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(2000)]
        public string Sql { get; set; }

        public virtual ICollection<User> Administrators { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}
