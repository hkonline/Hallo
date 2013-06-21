using System;
using System.Collections.Generic;

namespace Hallo.Models {
    public partial class Article {
        public int ArticleId { get; set; }
        public string Headline { get; set; }
        public string Author { get; set; }
        public System.DateTime Date { get; set; }
        public string FrontpageText { get; set; }
        public Nullable<int> FrontpageImageId { get; set; }
        public string Text { get; set; }
        public bool ApprovedByEditor { get; set; }
        public bool IsPublic { get; set; }
        public string ArticleType { get; set; }
        public Nullable<int> Category { get; set; }
        public Nullable<int> Category2 { get; set; }
        public virtual Image Image { get; set; }
    }
}
