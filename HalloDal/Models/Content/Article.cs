using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDal.Models.Content {
    public partial class Article {
        public int Id { get; set; }

        [Display(Name = "Headline", ResourceType = typeof(Resources.DisplayNames))]
        public string Headline { get; set; }

        [Display(Name = "Author", ResourceType = typeof(Resources.DisplayNames))]
        public string Author { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resources.DisplayNames))]
        public System.DateTime Date { get; set; }

        [Display(Name = "FrontpageText", ResourceType = typeof(Resources.DisplayNames))]
        public string FrontpageText { get; set; }

        [Display(Name = "FrontpageImage", ResourceType = typeof(Resources.DisplayNames))]
        public virtual Image FrontpageImage { get; set; }

        [Display(Name = "Text", ResourceType = typeof(Resources.DisplayNames))]
        public string Text { get; set; }

        [Display(Name = "ApprovedByEditor", ResourceType = typeof(Resources.DisplayNames))]
        public bool ApprovedByEditor { get; set; }

        [Display(Name = "IsPublic", ResourceType = typeof(Resources.DisplayNames))]
        public bool IsPublic { get; set; }

        public ArticleTypes ArticleType { get; set; }

        [Display(Name = "Categories", ResourceType = typeof(Resources.DisplayNames))]
        public virtual ICollection<ArticleCategory> Categories { get; set; }
        
        public virtual ICollection<Image> Images { get; set; }
        public long OldId { get; set; }
    }
}
