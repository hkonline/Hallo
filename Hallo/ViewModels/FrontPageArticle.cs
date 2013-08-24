using System;
using System.Linq;
using System.Configuration;
using HalloDal.Models.Content;

namespace Hallo.ViewModels {
    public class FrontPageArticle {

        public readonly Article Article;
        public static string ImageDirectoryUrl = ConfigurationManager.AppSettings["ImageDirectoryUrl"];            
            
        public FrontPageArticle(Article a) {
            Article = a;
        }

        public bool Newest = false;
        public bool HasFilm = false;
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

                return ImageDirectoryUrl + "/articleImages/thumbnails/img" + Article.FrontpageImage.OldId + ".jpg";
            }
            set { imageUrl = value; }
        }

        public string ImageLink {
            get {
                if (Article.FrontpageImage == null)
                    if (String.IsNullOrEmpty(ImageUrl))
                        return "";
                    else
                        return "<img class=aimg src='" + ImageUrl + "' hspace=3 width=200>";

                return "<img class=aimg src=\"" + ImageDirectoryUrl + "/articleImages/thumbnails/img" + Article.FrontpageImage.OldId + 
                    ".jpg\" hspace=3 width=200>";
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
                string headline = "<a class=titel href=";
                if (Newest) headline = "<a class=htitel href=";
                headline += ArticleLink + ">" + Article.Headline + "</a>";
                if (Article.IsPublic == false) headline += "<br /><div style=color:#FF0000; class=text>(Vises kun for venner med login)</div>";
                return headline;
            }
        }

        public string FrontpageHeadlineMobile {
            get {
                string headline = "<a href=";
                headline += ArticleLinkMobile + ">" + Article.Headline + "</a>";
                if (Article.IsPublic == false)
                    headline += "<br /><div style=color:#FF0000; class=text>(Vises kun for venner med login)</div>";
                return headline;
            }
        }
    }
}