using System;
using System.Linq;

namespace HalloDal.Models.Content {
    public class FrontpageLink {
        public int Id { get; set; }
        public string NavigationUrl { get; set; }
        public string Label { get; set; }
        public int Order { get; set; }
    }
}
