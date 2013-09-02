using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Hallo.ViewModels;
using System.Web;
using Hallo.Core;
using System.Configuration;
using System.IO;

namespace Hallo.Controllers {
    public class ImageController : HalloController {

        int ArticleId {
            get {
                object o = Session["CurrentArticleId"];
                return o == null ? 0 : (int)o;
            }
        }

        Article CurrentArticle {
            get {
                if (ArticleId == 0) return null;
                return Context.Articles.Include(x => x.FrontpageImage).Include(x=>x.Images).FirstOrDefault(x => x.Id == ArticleId);
            }
        }

        #region Imagelist
        public ActionResult List(int id) {
            Session["CurrentArticleId"] = id;

            List<ImageViewModel> model = new List<ImageViewModel>();
            List<Image> list = Context.Articles
                .First(x => x.Id == id)
                .Images
                .OrderBy(x => x.OrderNr)
                .ToList();

            foreach (Image i in list) {
                i.OrderNr = list.IndexOf(i);
                model.Add(new ImageViewModel(ArticleId, i) { 
                    IsFirst = list.IndexOf(i) == 0,
                    IsLast = list.IndexOf(i) == list.Count()-1
                });
            }
            Context.SaveChanges();

            return View(model);
        }

        [HttpPost]
        public ActionResult List(List<ImageViewModel> list) {
            foreach (ImageViewModel m in list) {
                m.IsFirst = m.OrderNr == 0;
                m.IsLast = m.OrderNr == list.Count - 1;
                Image image = CurrentArticle.Images.FirstOrDefault(x=>x.Id == m.Id);
                image.OrderNr = m.OrderNr;
                image.Description = m.Description;
            }
            Context.SaveChanges();
            ViewBag.ArticleId = ArticleId;
            return View(list);
        }

        /*public ActionResult OrderUp(int id) {
            Image image = Context.Images.FirstOrDefault(x => x.Id == id);
            int orderNr = (int)image.OrderNr;
            Image imageAbove = Context.Articles.FirstOrDefault(x => x.Id == ArticleId)
                .Images.FirstOrDefault(x => x.OrderNr == orderNr - 1);
            if (image != null && imageAbove != null) {
                image.OrderNr -= 1;
                imageAbove.OrderNr += 1;
                Context.SaveChanges();
            }
            return RedirectToAction("List", new { id = ArticleId });
        }

        public ActionResult OrderDown(int id) {
            Image image = Context.Images.FirstOrDefault(x => x.Id == id);
            int orderNr = (int)image.OrderNr;
            Image imageBelow = Context.Articles.FirstOrDefault(x => x.Id == ArticleId)
                .Images.FirstOrDefault(x => x.OrderNr == orderNr + 1);
            if (image != null && imageBelow != null) {
                image.OrderNr += 1;
                imageBelow.OrderNr -= 1;
                Context.SaveChanges();
            }
            return RedirectToAction("List", new { id = ArticleId });
        }*/

        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> files) {
            if (files != null) {
                foreach (HttpPostedFileBase file in files) {
                    Image i = new Image();
                    CurrentArticle.Images.Add(i);
                    Context.SaveChanges();
                    SaveToDisk(file, i.Id);
                }
            }
            return RedirectToAction("List", new { id = ArticleId });
        }

        public ActionResult Delete(int id) {
            Context.Images.Remove(Context.Images.FirstOrDefault(x => x.Id == id));
            Context.SaveChanges();
            int articleId = (int)Session["CurrentArticleId"];
            return RedirectToAction("List", new { id = articleId });
        }
        #endregion

        #region FrontpageImage
        public ActionResult DeleteFrontpageImage() {
            Article a = CurrentArticle;
            a.FrontpageImage = null;
            Context.SaveChanges();
            return RedirectToAction("FrontpageImage", new { id = ArticleId });
        }

        public ActionResult FrontpageImage(int id) {
            Session["CurrentArticleId"] = id;
            Image image = Context.Articles.Include(x => x.FrontpageImage).FirstOrDefault(x => x.Id == id).FrontpageImage;
            ImageViewModel model = new ImageViewModel( ArticleId, image);
            return View(model);
        }

        public ActionResult SaveDescription(string description) {
            Article a = CurrentArticle;
            a.FrontpageImage.Description = description;
            Context.SaveChanges();
            return RedirectToAction("FrontpageImage", new { id = ArticleId });
        }

        [HttpPost]
        public ActionResult FrontpageImage(int id, HttpPostedFileBase file) {
            if (file != null)
                SaveFrontpageImage(file);

            return RedirectToAction("FrontpageImage", new { id = ArticleId });
        }

        private void SaveFrontpageImage(HttpPostedFileBase file) {
            Article a = Context.Articles.FirstOrDefault(x => x.Id == ArticleId);
            a.FrontpageImage = new Image();
            Context.SaveChanges();
            int imageId = a.FrontpageImage.Id;
            SaveToDisk(file, imageId);
        }
        #endregion

        private void SaveToDisk(HttpPostedFileBase file, int imageId) {
            ImageHelper helper = new ImageHelper(file.InputStream);

            System.Drawing.Image thumb = helper.GetResizedImage(200);

            MemoryStream ms = new MemoryStream();
            thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            System.IO.File.WriteAllBytes(
                Server.MapPath("~/") +
                ConfigurationManager.AppSettings["ImageDirectoryUrl"].Substring(1) +
                "/thumbnails/img" + imageId + ".jpg",
                ms.ToArray()
            );

            System.Drawing.Image image;
            if (thumb.Width > thumb.Height) {
                image = helper.GetResizedImage(720);
            } else {
                image = helper.GetResizedImage(480);
            }

            ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            System.IO.File.WriteAllBytes(
                Server.MapPath("~/") +
                ConfigurationManager.AppSettings["ImageDirectoryUrl"].Substring(1) +
                "/images/img" + imageId + ".jpg",
                ms.ToArray()
            );
        }

        public void Slideshow(int articleId) { 
        
        }
    }
}
