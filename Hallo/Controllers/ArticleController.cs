using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using HalloDal.Models.Content;
using Hallo.ViewModels;

namespace Hallo.Controllers {
    public class ArticleController : HalloController {

        private List<ImageViewModel> GetImages(int articleId) {
            List<Image> list = Context.Articles.Include(m => m.Images).First(x => x.Id == articleId).Images.OrderBy(x => x.OrderNr).ToList();

            List<ImageViewModel> imageViewModels = new List<ImageViewModel>();
            foreach (Image i in list) imageViewModels.Add(new ImageViewModel(articleId, i));

            return imageViewModels;
        }

        public ActionResult Article(int id) {
            ViewBag.ShowLeft = true;

            ArticleViewModel model = new ArticleViewModel() {
                Article = Context.Articles.Where(x => x.Id == id).Include(x => x.FrontpageImage).SingleOrDefault(),
                Images = GetImages(id)
            };

            Session["CurrentArticleViewModel"] = model;

            model.FrontPageImage = new ImageViewModel(id, model.Article.FrontpageImage);

            return View(model);
        }

        public ActionResult List() {
            return View(Context.Articles.OrderByDescending(x => x.Date).Take(10).ToList());
        }

        private Article GetArticle(int id) {
            return Context.Articles
                .Where(x => x.Id == id)
                .Include(x => x.Images)
                .Include(x => x.FrontpageImage)
                .Include(x => x.Category)
                .Include(x => x.Category2)
                .SingleOrDefault();
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            return View(GetArticle(id));
        }

        [HttpGet]
        public ActionResult Create() {
            Article newArticle = new Article() {
                Date = DateTime.Now,
            };

            Context.Articles.Add(newArticle);
            Context.SaveChanges();

            return RedirectToAction("Edit", new { id = newArticle.Id });
        }

        public ActionResult Delete(int id) {
            Context.Articles.Remove(GetArticle(id));
            Context.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article article) {
            Article dbArticle = GetArticle(article.Id);

            dbArticle.Headline = article.Headline;
            dbArticle.Author = article.Author;
            dbArticle.FrontpageText = article.FrontpageText;
            dbArticle.Text = article.Text;

            Context.SaveChanges();

            return View(dbArticle);
        }

        public JsonResult SetApproved(int id, bool approved) {
            Article a = GetArticle(id);
            a.ApprovedByEditor = approved;
            a.Date = DateTime.Now;
            Context.SaveChanges();
            return Json(null);
        }

        public JsonResult SetPublic(int id, bool isPublic) {
            Article a = GetArticle(id);
            a.IsPublic = isPublic;
            Context.SaveChanges();
            return Json(null);
        }
    }
}
