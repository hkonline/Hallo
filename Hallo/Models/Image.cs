using System;
using System.Collections.Generic;

namespace Hallo.Models
{
    public partial class Image
    {
        public Image()
        {
            this.Articles = new List<Article>();
        }

        public int ImageID { get; set; }
        public int ArticleId { get; set; }
        public string Description { get; set; }
        public Nullable<int> OrderNr { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
