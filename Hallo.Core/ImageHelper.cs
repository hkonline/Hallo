using System;
using System.Drawing;
using System.IO;

namespace Hallo.Core {
    public class ImageHelper {

        readonly ImageResizer resizer;

        public ImageHelper(Stream stream) {
            resizer = new ImageResizer() {
                Image = Image.FromStream(stream),
            };
        }

        public Image GetResizedImage(int width) {
            resizer.Width = width;
            return resizer.GetThumbnail();
        }

    }
}
