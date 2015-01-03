using System;
using System.Linq;
using System.Configuration;

namespace Hallo {
    public class Constant {
        public const string WebmasterMail = "webmaster@hkonline.dk";
        
        public static string[] Months = { 
            "januar", "februar", "marts", "april", "maj", "juni", 
		    "juli", "august", "september", "oktober", "november", "december" 
        };

        // We save the URL in this static variable just to save a little performance.
        private static string imageDirectoryUrl;
        public static string ImageDirectoryUrl {
            get {
                if (imageDirectoryUrl == null)
                    imageDirectoryUrl = ConfigurationManager.AppSettings["ImageDirectoryUrl"].Substring(1);
                return imageDirectoryUrl;
            }
        }

        // We save the URL in this static variable just to save a little performance.
        private static int thumbnailWidth;
        public static int ThumbnailWidth {
            get {
                if (thumbnailWidth == 0)
                    thumbnailWidth = int.Parse(ConfigurationManager.AppSettings["ThumbnailWidth"]);
                return thumbnailWidth;
            }
        }
    }
}