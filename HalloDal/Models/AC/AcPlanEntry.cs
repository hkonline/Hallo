using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDal.Models.AC {
    public class AcPlanEntry {

        [Key]
        public int Id { get; set; }
        public int TeamId { get; set; }

        [Display(Name = "AcDate", ResourceType = typeof(Resources.DisplayNames))]
        public AcDate Date { get; set; }

        [Display(Name = "AcActivity", ResourceType = typeof(Resources.DisplayNames))]
        public String Activity { get; set; }

        [Display(Name = "Remember", ResourceType = typeof(Resources.DisplayNames))]
        public String Remember { get; set; }


    }
}
