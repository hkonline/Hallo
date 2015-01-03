using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDal.Models.Users {
    public class LogEntry {

        public long LogEntryId { get; set; }
        
        public int PmoId { get; set; }
        
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(40)]
        public string FirstName { get; set; }

        [StringLength(40)]
        public string LastName { get; set; }

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

    }
}
