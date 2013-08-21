using System;
using System.Collections.Generic;

namespace HalloDal.Models.Content {
    public partial class Image {
        public int Id { get; set; }
        public string Description { get; set; }
        public Nullable<int> OrderNr { get; set; }
        public Nullable<int> OldId { get; set; }
    }
}
