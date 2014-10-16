using System;
using System.Linq;
using System.Configuration;
using HalloDal.Models.Content;
using System.Web;

namespace Hallo.ViewModels {
    public class FrontPageArticle {

        public readonly Article Article;
        public int ImageId {
            get {
                if (Article.FrontpageImage == null) return 0;
                return (int)(Article.FrontpageImage.OldId == null ? Article.FrontpageImage.Id : Article.FrontpageImage.OldId);
            }
        }


        private static string imageDirectoryUrl;
        public static string ImageDirectoryUrl {
            get {
                if (imageDirectoryUrl == null)
                    imageDirectoryUrl = 
                        //HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                        //HttpRuntime.AppDomainAppVirtualPath +
                        ConfigurationManager.AppSettings["ImageDirectoryUrl"].Substring(1);

                return imageDirectoryUrl;
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

        private string imageUrl;
        public string ImageUrl {
            get {
                if (imageUrl != null && imageUrl.Length > 0) return imageUrl;

                if (Article.FrontpageImage == null) return "";

                return ImageDirectoryUrl + "/thumbnails/img" + ImageId + ".jpg";
            }
            set { imageUrl = value; }
        }

        public string ImageLink {
            get {
                if (Article.FrontpageImage == null)
                    if (String.IsNullOrEmpty(ImageUrl))
                        return "";
                    else
                        return "<img class='img-thumbnail pull-right' src='" + ImageUrl + "'>";

                return "<img class='img-thumbnail pull-right' src=\"" + ImageDirectoryUrl + "/thumbnails/img" + ImageId + ".jpg\">";
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

        public string FrontpageHeadline {
            get {
                int headerNum = 3;
                if (Newest) headerNum = 2;

                string headline = "<a href=" + ArticleLink + "><h" + (Newest ? (headerNum.ToString() + " style='margin-top: 0px;'") : headerNum.ToString()) + ">" + 
                    Article.Headline + "</h" + headerNum + "></a>";
                if (Article.IsPublic == false) headline += "<div class='text-info'>(Vises kun for venner med login)</div>";
                return headline;
            }
        }

        public string FrontpageHeadlineMobile {
            get {
                string headline = "<a href=";
                headline += ArticleLinkMobile + ">" + Article.Headline + "</a>";
                if (Article.IsPublic == false)
                    headline += "<br /><div class='text-info'>(Vises kun for venner med login)</div>";
                return headline;
            }
        }
    }
}