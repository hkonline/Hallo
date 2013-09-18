using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hallo.ViewModels {
    public class AssignedArticleCategory {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}