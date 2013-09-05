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
                return Context.Articles.Include(x => x.FrontpageImage).Include(x => x.Images).FirstOrDefault(x => x.Id == ArticleId);
            }
        }

        #region Imagelist
        [HttpGet]
        public ActionResult List(int id) {
            List<Image> list = GetImages(id);
            List<ImageViewModel> model = new List<ImageViewModel>();
            ViewBag.ArticleId = id;

            foreach (Image i in list) {
                i.OrderNr = list.IndexOf(i);
                model.Add(new ImageViewModel(ArticleId, i) {
                    IsFirst = list.IndexOf(i) == 0,
                    IsLast = list.IndexOf(i) == list.Count() - 1
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
                Image image = CurrentArticle.Images.FirstOrDefault(x => x.Id == m.Id);
                image.OrderNr = m.OrderNr;
                image.Description = m.Description;
            }
            Context.SaveChanges();
            ViewBag.ArticleId = ArticleId;
            return View(list);
        }

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
            ImageViewModel model = new ImageViewModel(ArticleId, image);
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

        private List<Image> GetImages(int articleId) {
            Session["CurrentArticleId"] = articleId;

            return Context.Articles.Include(m=>m.Images)
                .First(x => x.Id == articleId)
                .Images
                .OrderBy(x => x.OrderNr)
                .ToList();
        }

        public ActionResult Slideshow(int articleId) {
            List<Image> list = GetImages(articleId);
            List<ImageViewModel> model = new List<ImageViewModel>();
            foreach (Image i in list) model.Add(new ImageViewModel(ArticleId, i));
            return View(model);
        }
    }
}
