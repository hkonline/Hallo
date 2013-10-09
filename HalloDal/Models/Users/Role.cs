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
        
        [DisplayName("Rolle name")]
        [StringLength(50)]
        public string RoleName { get; set; }

        [DisplayName("Rolle beskrivelse")]
        [StringLength(100)]
        public string Description { get; set; }

        public virtual IList<User> Users { get; set; }
    }
}
