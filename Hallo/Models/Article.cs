using System;
using System.Collections.Generic;

namespace Hallo.Models {
    public partial class Article {
        public long ArtikelID { get; set; }
        public string Overskrift { get; set; }
        public string Forfatter { get; set; }
        public Nullable<System.DateTime> Dato { get; set; }
        public string URL { get; set; }
        public string ForsideTekst { get; set; }
        public string ForsideBilledeURL { get; set; }
        public Nullable<int> ForsideBilledeID { get; set; }
        public string Tekst { get; set; }
        public Nullable<bool> HarKant { get; set; }
        public Nullable<bool> ErAutomatiseret { get; set; }
        public Nullable<int> Layout { get; set; }
        public Nullable<bool> IsCheckedByJens { get; set; }
        public bool PublicArticle { get; set; }
        public bool IsSlideshow { get; set; }
        public string ArticleType { get; set; }
        public Nullable<int> Category { get; set; }
        public Nullable<int> Category2 { get; set; }
    }
}
