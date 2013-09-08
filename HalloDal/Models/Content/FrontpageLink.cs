using System;
using System.Linq;

namespace HalloDal.Models.Content {

    // Links to external sites, shown in the frontpage
    public class FrontpageLink {
        public int Id { get; set; }
        public string NavigationUrl { get; set; }
        public string Label { get; set; }
        public int Order { get; set; }
    }
}
