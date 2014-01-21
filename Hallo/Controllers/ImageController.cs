using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Hallo.ViewModels;
using System.Web;

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
                return db.Articles.Include(x => x.FrontpageImage).Include(x => x.Images).FirstOrDefault(x => x.Id == ArticleId);
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
            db.SaveChanges();

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
            db.SaveChanges();
            ViewBag.ArticleId = ArticleId;
            return View(list);
        }

        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> files) {
            if (files != null) {
                foreach (HttpPostedFileBase file in files) {
                    Image i = new Image();
                    CurrentArticle.Images.Add(i);
                    db.SaveChanges();
                    SaveImageToDisk(file, i);
                }
            }
            return RedirectToAction("List", new { id = ArticleId });
        }

        public ActionResult Delete(int id) {
            db.Images.Remove(db.Images.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
            int articleId = (int)Session["CurrentArticleId"];
            return RedirectToAction("List", new { id = articleId });
        }
        #endregion

        #region FrontpageImage
        public ActionResult DeleteFrontpageImage() {
            Article a = CurrentArticle;
            a.FrontpageImage = null;
            db.SaveChanges();
            return RedirectToAction("FrontpageImage", new { id = ArticleId });
        }

        public ActionResult FrontpageImage(int id) {
            Session["CurrentArticleId"] = id;
            Image image = db.Articles.Include(x => x.FrontpageImage).FirstOrDefault(x => x.Id == id).FrontpageImage;
            ImageViewModel model = new ImageViewModel(ArticleId, image);
            return View(model);
        }

        public ActionResult SaveDescription(string description) {
            Article a = CurrentArticle;
            a.FrontpageImage.Description = description;
            db.SaveChanges();
            return RedirectToAction("FrontpageImage", new { id = ArticleId });
        }

        [HttpPost]
        public ActionResult FrontpageImage(int id, HttpPostedFileBase file) {
            if (file != null)
                SaveFrontpageImage(file);

            return RedirectToAction("FrontpageImage", new { id = ArticleId });
        }

        private void SaveFrontpageImage(HttpPostedFileBase file) {
            Article a = db.Articles.FirstOrDefault(x => x.Id == ArticleId);
            a.FrontpageImage = new Image();
            db.SaveChanges();
            SaveImageToDisk(file, a.FrontpageImage);
        }
        #endregion

        public List<Image> GetImages(int articleId) {
            Session["CurrentArticleId"] = articleId;

            return db.Articles.Include(m => m.Images)
                .First(x => x.Id == articleId)
                .Images
                .OrderBy(x => x.OrderNr)
                .ToList();
        }

        public ActionResult Slideshow(int id, int orderNr) {
            ArticleViewModel m = (ArticleViewModel)Session["CurrentArticleViewModel"];
            
            ImageViewModel image;
            if (orderNr >= 0) {
                image = m.Images.First(i => i.OrderNr == orderNr);
            } else
                image = new ImageViewModel(id, m.Article.FrontpageImage);

            image.IsFirst = orderNr == -1;
            image.IsLast = orderNr == (m.Images.Count() - 1);
            
            return View(image);
        }
    }
}
