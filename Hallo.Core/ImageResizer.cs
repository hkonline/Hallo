using System;
using System.Linq;
using System.Drawing;
using System.IO;

namespace Hallo.Core {
    public class ImageResizer {

        private bool mUseAspect = true;
        private string mImagePath = "";
        private Image mSrcImage, mDstImage;
        private ImageResizer mCache;
        private Graphics mGraphics;

        public string File {
            get {
                return mImagePath;
            }
            set { mImagePath = value; }
        }

        public Image Image {
            get { return mSrcImage; }
            set { mSrcImage = value; }
        }

        public bool PreserveAspectRatio {
            get { return mUseAspect; }
            set { mUseAspect = value; }
        }

        public bool UsePercentages { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        protected virtual bool IsSameSrcImage(ImageResizer other) {
            if (other != null) {
                return (File == other.File
                    && Image == other.Image);
            }

            return false;
        }

        protected virtual bool IsSameDstImage(ImageResizer other) {
            if (other != null) {
                return (Width == other.Width
                    && Height == other.Height
                    && UsePercentages == other.UsePercentages
                    && PreserveAspectRatio == other.PreserveAspectRatio);
            }

            return false;
        }

        public virtual Image GetThumbnail() {
            // Flag whether a new image is required
            bool recalculate = false;
            double newWidth = Width;
            double newHeight = Height;

            // Is there a cached source image available? If not,
            // load the image if a filename was specified; otherwise
            // use the image in the Image property
            if (!IsSameSrcImage(mCache)) {
                if (mImagePath.Length > 0) {
                    // Load via stream rather than Image.FromFile to release the file
                    // handle immediately
                    if (mSrcImage != null)
                        mSrcImage.Dispose();

                    // Wrap the FileStream in a "using" directive, to ensure the handle
                    // gets closed when the object goes out of scope
                    using (Stream stream = new FileStream(mImagePath, FileMode.Open))
                        mSrcImage = Image.FromStream(stream);

                    recalculate = true;
                }
            }

            // Check whether the required destination image properties have
            // changed
            if (!IsSameDstImage(mCache)) {
                // Yes, so we need to recalculate.
                // If you opted to specify width and height as percentages of the original
                // image's width and height, compute these now
                if (UsePercentages) {
                    if (Width != 0) {
                        newWidth = mSrcImage.Width * Width / 100;

                        if (PreserveAspectRatio) {
                            newHeight = newWidth * mSrcImage.Height / mSrcImage.Width;
                        }
                    }

                    if (Height != 0) {
                        newHeight = mSrcImage.Height * Height / 100;

                        if (PreserveAspectRatio) {
                            newWidth = newHeight * mSrcImage.Width / mSrcImage.Height;
                        }
                    }
                } else {
                    // If you specified an aspect ratio and absolute width or height, then calculate this 
                    // now; if you accidentally specified both a width and height, ignore the 
                    // PreserveAspectRatio flag
                    if (PreserveAspectRatio) {
                        if (Width != 0 && Height == 0) {
                            newHeight = (Width / mSrcImage.Width) * mSrcImage.Height;
                        } else if (Height != 0 && Width == 0) {
                            newWidth = (Height / mSrcImage.Height) * mSrcImage.Width;
                        }
                    }
                }

                recalculate = true;
            }

            if (recalculate) {
                // Calculate the new image
                if (mDstImage != null) {
                    mDstImage.Dispose();
                    mGraphics.Dispose();
                }

                Bitmap bitmap = new Bitmap((int)newWidth, (int)newHeight, mSrcImage.PixelFormat);
                mGraphics = Graphics.FromImage(bitmap);
                mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                mGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                mGraphics.DrawImage(mSrcImage, 0, 0, bitmap.Width, bitmap.Height);
                mDstImage = bitmap;

                // Cache the image and its associated settings
                mCache = MemberwiseClone() as ImageResizer;
            }

            return mDstImage;
        }

        /*        ~ImageResizer() {
                    // Free resources
                    if (m_dst_image != null) {
                        m_dst_image.Dispose();
                        m_graphics.Dispose();
                    }

                    if (m_src_image != null)
                        m_src_image.Dispose();
                }*/

    }
}
