using HalloDal.Models.Content;
using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hallo.Images {
    public class ImageFinder {

        private static int FileId(Image i) {
            return (int)(i.OldId == null ? i.Id : i.OldId);
        }

        public static string GetImageUrl(Image i) {
            return Constant.ImageDirectoryUrl + "/images/img" + FileId(i) + ".jpg";
        }

        public static string GetThumbUrl(Image i) {
            return Constant.ImageDirectoryUrl + "/thumbnails/img" + FileId(i) + ".jpg";
        }        
    }
}