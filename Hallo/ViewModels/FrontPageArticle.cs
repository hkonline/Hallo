using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hallo.Models;

namespace Hallo.ViewModels {
    public class FrontPageArticle {

        public readonly Article article;

        public FrontPageArticle(Article a) {
            article = a;
        }

        public bool newest = false;
        public bool hasFilm = false;
        public string filmLink {
            get {
                return hasFilm ? "&nbsp;&nbsp;<img src=\"/images/filmklip.jpg\" height=\"20\" width=\"20\" />" : "";
            }
        }

        public string DatoString {
            get {
                return ((DateTime)article.Date).Day + ". " +
                    Constant.months[((DateTime)article.Date).Month - 1].Substring(0, 3) + " " +
                    ((DateTime)article.Date).Year;
            }
        }

        private string imageURL;
        public string ImageURL {
            get {
                if (imageURL != null && imageURL.Length > 0) return imageURL;

                if (article.FrontpageImageId == 0) return "";

                return "/images/articleImages/thumbnails/img" + article.FrontpageImageId + ".jpg";
            }
            set { imageURL = value; }
        }

        public string ImageLink {
            get {
                if (article.FrontpageImageId == 0)
                    if (String.IsNullOrEmpty(ImageURL))
                        return "";
                    else
                        return "<img class=aimg src='" + ImageURL + "' hspace=3 width=200>";

                return "<img class=aimg src='/images/articleImages/thumbnails/img" + article.FrontpageImageId + ".jpg' hspace=3 width=200>";
            }
        }
        public string ArticleLink {
            get {
                return "/Home/Article/" + article.ArticleId;
            }
        }

        public string ArticleLinkMobile {
            get {
                return "'Article.aspx?articleID=" + article.ArticleId + "'";
            }
        }

        public string FrontpageHeadline {
            get {
                string headline = "<a class=titel href=";
                if (newest) headline = "<a class=htitel href=";
                headline += ArticleLink + ">" + article.Headline + "</a>";
                if (article.IsPublic == false) headline += "<br /><div style=color:#FF0000; class=text>(Vises kun for venner med login)</div>";
                return headline;
            }
        }

        public string FrontpageHeadlineMobile {
            get {
                string headline = "<a href=";
                headline += ArticleLinkMobile + ">" + article.Headline + "</a>";
                if (article.IsPublic == false)
                    headline += "<br /><div style=color:#FF0000; class=text>(Vises kun for venner med login)</div>";
                return headline;
            }
        }
    }
}