using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDal.Models.Users {
    public partial class User {        
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MobilPhone { get; set; }
        public string Email { get; set; }
        public Nullable<bool> WantInfoSms { get; set; }
        public Nullable<bool> WantInfoMail { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<int> Father { get; set; }
        public Nullable<int> Mother { get; set; }
        public Nullable<bool> Approved { get; set; }
        public string LocalBccAccount { get; set; }
        public Nullable<int> LbuNumber { get; set; }
        public Nullable<int> Spouse { get; set; }
        public string Gender { get; set; }
        public Nullable<bool> LiveInLocalChurch { get; set; }
        public Nullable<bool> AttendsMeetings { get; set; }

        public IList<Role> Roles { get; set; }
    }
}
