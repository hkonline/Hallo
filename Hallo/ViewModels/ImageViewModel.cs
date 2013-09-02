using System;
using System.Linq;
using HalloDal.Models.Content;
using Hallo.Images;

namespace Hallo.ViewModels {
    public class ImageViewModel {

        public int Id {
            get { return Image.Id; }
            set { Image.Id = value; }
        }

        public string Description {
            get { return Image.Description; }
            set { Image.Description = value; }
        }

        public int OrderNr {
            get {
                if (Image.OrderNr == null) return 0;
                return (int) Image.OrderNr; 
            }
            set { Image.OrderNr = value; }
        }

        public ImageViewModel(int articleId, Image image) {
            ArticleId = articleId;
            Image = image;
        }

        public ImageViewModel() {
            Image = new Image();
        }

        public int ArticleId { get; set; }

        public Image Image { get; set; }

        public string GetImageUrl() {
            return ImageFinder.GetImageUrl(Image);
        }

        public string GetThumbUrl() {
            return ImageFinder.GetThumbUrl(Image);
        }

        public bool IsFirst;
        public bool IsLast;
    }
}