using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDal.Models.Users {
    public partial class User {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(40)]
        public string Firstname { get; set; }

        [StringLength(40)]
        public string Lastname { get; set; }

        [StringLength(20)]
        public string MobilPhone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        public Nullable<bool> WantInfoSms { get; set; }
        public Nullable<bool> WantInfoMail { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<int> Guardian1 { get; set; }
        public Nullable<int> Guardian2 { get; set; }
        public Nullable<bool> Approved { get; set; }

        [StringLength(20)]
        public string LocalBccAccount { get; set; }

        public Nullable<int> LbuNumber { get; set; }
        public Nullable<int> Spouse { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }
        [StringLength(50)]
        public string Country { get; set; }

        public int ChurchId { get; set; }

        [StringLength(50)]
        public string ChurchName { get; set; }

        public int HomeChurchId { get; set; }

        [StringLength(50)]
        public string HomeChurchName { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<UserGroup> AdminUserGroups { get; set; }

        public bool IsInPmo { get; set; }
        public bool Authorized { get; set; }

        public int NumberOfVisits { get; set; }
        public Nullable<System.DateTime> LastVisit { get; set; }
    }
}
