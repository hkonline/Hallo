using System;
using System.Collections.Generic;

namespace Hallo.Models
{
    public partial class InfoBox
    {
        public int id { get; set; }
        public Nullable<int> type { get; set; }
        public string overskrift { get; set; }
        public string forfatter { get; set; }
        public string info { get; set; }
        public Nullable<System.DateTime> dato_begynd { get; set; }
        public Nullable<System.DateTime> dato_slut { get; set; }
        public bool isPublic { get; set; }
    }
}
