using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HalloDal.Models.Content {
    public class HalloFile {
        public int Id { get; set; }
        
        [Display(Name = "Description", ResourceType = typeof(Resources.DisplayNames))]
        public string Description { get; set; }

        public string Extension { get; set; }    
    }
}
