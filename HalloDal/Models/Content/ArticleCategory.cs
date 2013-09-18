using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HalloDal.Models.Content {
    public class ArticleCategory {

        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
