using System;
using System.ComponentModel;

namespace HalloDal.Models.Content {
    public partial class Image {
        public int Id { get; set; }

        [DisplayName("Beskrivelse")]
        public string Description { get; set; }
        public Nullable<int> OrderNr { get; set; }
        public Nullable<int> OldId { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> Width { get; set; }
    }
}
