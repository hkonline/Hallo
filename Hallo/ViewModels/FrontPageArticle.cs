using System;
using System.Linq;
using System.Configuration;
using HalloDal.Models.Content;
using System.Web;

namespace Hallo.ViewModels {
    public class FrontPageArticle {

        public readonly Article Article;

        // Returns the frontpage image id
        public int ImageId {
            get {
                if (Article.FrontpageImage == null) return 0;
                return (int)(Article.FrontpageImage.OldId == null ? Article.FrontpageImage.Id : Article.FrontpageImage.OldId);
            }
        }

        public string ImageDirectoryUrl {
            get {
                return Constant.ImageDirectoryUrl;
            }
        }

        public FrontPageArticle(Article a) {
            Article = a;
        }

        public bool Newest = false;
        public bool HasFilm = false;
        public bool HasFrontPageImage { get { return Article.FrontpageImage != null; } }

        public string FilmLink {
            get {
                return HasFilm ? "&nbsp;&nbsp;<img src=\"/images/filmklip.jpg\" height=\"20\" width=\"20\" />" : "";
            }
        }

        public string DatoString {
            get {
                return Article.Date.Day + ". " +
                    Constant.Months[Article.Date.Month - 1].Substring(0, 3) + " " + Article.Date.Year;
            }
        }
               
        public string ArticleLink {
            get {
                return "/Article/Article/" + Article.Id;
            }
        }

        public string ArticleLinkMobile {
            get {
                return "'Article.aspx?articleID=" + Article.Id + "'";
            }
        }        
    }
}