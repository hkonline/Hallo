using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDal.Models.Content {
    public partial class Image {
        public int Id { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.DisplayNames))]
        public string Description { get; set; }
        public Nullable<int> OrderNr { get; set; }
        public Nullable<int> OldId { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> Width { get; set; }
    }
}
