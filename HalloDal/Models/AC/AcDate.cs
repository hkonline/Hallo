using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDal.Models.AC {
    public class AcDate {

        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

    }
}
