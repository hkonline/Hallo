using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hallo.ViewModels {
    public class AcPlanViewModel {
        
        public int Id { get; set; }
        public int DateId { get; set; }
        public int TeamId { get; set; }
        public DateTime Date { get; set; }
        public int MyProperty { get; set; }
        public string Activity { get; set; }
        public string Remember { get; set; }

    }
}