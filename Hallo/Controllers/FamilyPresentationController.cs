using Hallo.ViewModels;
using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class FamilyPresentationController : HalloController {

        private Article GetArticle(int id) {
            return db.Articles
                .Where(x => x.Id == id)
                .Include(x => x.Images)
                .Include(x => x.FrontpageImage)
                .Include(x => x.Categories)
                .SingleOrDefault();
        }

        private List<ImageViewModel> GetImages(int articleId) {
            List<Image> list = db.Articles.Include(m => m.Images).First(x => x.Id == articleId).Images.OrderBy(x => x.OrderNr).ToList();

            List<ImageViewModel> imageViewModels = new List<ImageViewModel>();
            foreach (Image i in list) imageViewModels.Add(new ImageViewModel(articleId, i));

            return imageViewModels;
        }

        public ActionResult Article(int id) {
            ViewBag.ShowLeft = true;

            ArticleViewModel model = new ArticleViewModel() {
                Article = db.Articles.Where(x => x.Id == id).Include(x => x.FrontpageImage).SingleOrDefault(),
                Images = GetImages(id)
            };

            Session["CurrentArticleViewModel"] = model;

            model.FrontPageImage = new ImageViewModel(id, model.Article.FrontpageImage);

            return View(model);
        }

        public ActionResult List() {
            Authorize("Editor", "Journalist");
            ViewBag.User = HalloUser;

            return View(db.Articles.Where(x => x.ArticleType == ArticleTypes.FamilyPresentation).OrderByDescending(x => x.Date).ToList());
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            Authorize("Editor", "Journalist");

            Article article = GetArticle(id);
            return View(article);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article article, String[] selectedCategories) {
            Article dbArticle = GetArticle(article.Id);
            dbArticle.Headline = article.Headline;
            dbArticle.Text = article.Text;
            db.SaveChanges();
            return View(dbArticle);
        }

        [HttpGet]
        public ActionResult Create() {
            Authorize("Editor", "Journalist");

            Article newArticle = new Article() {
                Date = DateTime.Now, ArticleType = ArticleTypes.FamilyPresentation
            };

            db.Articles.Add(newArticle);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = newArticle.Id });
        }

        public ActionResult Delete(int id) {
            db.Articles.Remove(GetArticle(id));
            db.SaveChanges();

            return RedirectToAction("List");
        }

    }
}
