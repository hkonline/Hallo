using System;
using System.Collections.Generic;
using System.Linq;

namespace Hallo.ViewModels {
    public class HKMenuItem {
        public string Text { get; set; }
        public string Url { get; set; }
        public List<HKMenuItem> SubMenu { get; set; }
        public bool Active { get; set; }
        public string LiClass { get { if (Active) return "k-state-active"; else return ""; } }
    }
}