using HalloDal.Models.Content;
using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hallo.Images {
    public class ImageFinder {
        private static string imageDirectoryUrl;
        private static string ImageDirectoryUrl {
            get {
                if (imageDirectoryUrl == null)
                    imageDirectoryUrl =
                        HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                        HttpRuntime.AppDomainAppVirtualPath +
                        ConfigurationManager.AppSettings["ImageDirectoryUrl"];

                return imageDirectoryUrl;
            }
        }

        private static int FileId(Image i) {
            return (int)(i.OldId == null ? i.Id : i.OldId);
        }

        public static string GetImageUrl(Image i) {
            return ImageDirectoryUrl + "/images/img" + FileId(i) + ".jpg";
        }

        public static string GetThumbUrl(Image i) {
            return ImageDirectoryUrl + "/thumbnails/img" + FileId(i) + ".jpg";
        }

    }
}