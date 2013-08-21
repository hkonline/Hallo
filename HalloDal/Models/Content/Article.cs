using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace HalloDal.Models.Content {
    public partial class Article {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Author { get; set; }
        public System.DateTime Date { get; set; }
        public string FrontpageText { get; set; }
        public virtual Image FrontpageImage { get; set; }
        public string Text { get; set; }
        public bool ApprovedByEditor { get; set; }
        public bool IsPublic { get; set; }
        public ArticleTypes ArticleType { get; set; }
        public virtual ArticleCategory Category { get; set; }
        public virtual ArticleCategory Category2 { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public long OldId { get; set; }
    }
}
