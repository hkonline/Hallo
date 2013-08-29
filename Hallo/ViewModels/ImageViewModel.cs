using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using HalloDal.Models.Content;
using System.Web;

namespace Hallo.ViewModels {
    public class ImageViewModel {
        public ImageViewModel(int id) {
            ArticleId = id;
        }

        public int ArticleId { get; private set; }
        public List<Image> Images { get; set; }

        private static string imageDirectoryUrl;
        private string ImageDirectoryUrl {
            get {
                if (imageDirectoryUrl == null)
                    imageDirectoryUrl =
                        HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                        HttpRuntime.AppDomainAppVirtualPath +
                        ConfigurationManager.AppSettings["ImageDirectoryUrl"];

                return imageDirectoryUrl;
            }
        }

        public string GetImageUrl(Image image) {
            return ImageDirectoryUrl + "/images/img" + image.Id + ".jpg";
        }

        public string GetThumbUrl(Image image) {
            return ImageDirectoryUrl + "/thumbnails/img" + image.Id + ".jpg";
        }
    }
}