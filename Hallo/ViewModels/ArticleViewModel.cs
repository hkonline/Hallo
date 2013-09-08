using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hallo.ViewModels {
    public class ArticleViewModel {
        public Article Article { get; set; }
        public ImageViewModel FrontPageImage { get; set; }
        public List<ImageViewModel> Images { get; set; }
    }
}