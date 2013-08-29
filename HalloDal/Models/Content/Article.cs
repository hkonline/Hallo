using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HalloDal.Models.Content {
    public partial class Article {
        public int Id { get; set; }
        
        [DisplayName("Overskrift")]
        public string Headline { get; set; }

        [DisplayName("Forfatter")]
        public string Author { get; set; }

        [DisplayName("Dato")]
        public System.DateTime Date { get; set; }

        [DisplayName("Forsidetekst")]
        public string FrontpageText { get; set; }

        [DisplayName("Forsidebillede")]
        public virtual Image FrontpageImage { get; set; }
        public string Text { get; set; }
        
        [DisplayName("Godkendt")]
        public bool ApprovedByEditor { get; set; }

        [DisplayName("Offentlig")]
        public bool IsPublic { get; set; }

        public ArticleTypes ArticleType { get; set; }
        public virtual ArticleCategory Category { get; set; }
        public virtual ArticleCategory Category2 { get; set; }
        
        public virtual ICollection<Image> Images { get; set; }
        public long OldId { get; set; }
    }
}
