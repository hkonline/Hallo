using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDal.Models.Users {
    public class Role {
        public int RoleId { get; set; }

        [Display(Name = "RoleName", ResourceType = typeof(Resources.DisplayNames))]
        [StringLength(50)]
        public string RoleName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.DisplayNames))]
        [StringLength(100)]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
