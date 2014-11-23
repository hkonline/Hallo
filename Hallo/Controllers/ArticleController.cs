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
            List<Image> list = db.Articles.Include(m => m.Images).First(x => x.Id == articleId).Images.OrderBy(x => x.OrderNr).ToList();

            List<ImageViewModel> imageViewModels = new List<ImageViewModel>();
            foreach (Image i in list) imageViewModels.Add(new ImageViewModel(articleId, i));

            return imageViewModels;
        }

        // Recursive function replaceing <file> tags with file-links
        private string InsertFiles(string s) {
            int p1 = s.ToLower().IndexOf("&lt;file");
            if (p1 < 0) return s;
            int p2 = s.Substring(p1).IndexOf("&gt;");
            int id;
            if (Int32.TryParse(s.Substring(p1 + 8, p2 - 8), out id)) {
                string s2 = s.Substring(0, p1) + GetFileUrl(id) + s.Substring(p1 + p2 + 4);
                return InsertFiles(s2);
            } else return s;
        }

        private string GetFileUrl(int id) {
            HalloFile dbFile = db.Files.Find(id);
            return (new FileViewModel(dbFile)).Url;        
        }

        public ActionResult Article(int id) {
            ViewBag.ShowLeft = true;

            ArticleViewModel model = new ArticleViewModel() {
                Article = db.Articles.Where(x => x.Id == id).Include(x => x.FrontpageImage).SingleOrDefault(),
                Images = GetImages(id)
            };
            
            if (model.Article.Text == null) model.Article.Text = "";
            
            model.Article.Text = InsertFiles(model.Article.Text);

            Session["CurrentArticleViewModel"] = model;

            model.FrontPageImage = new ImageViewModel(id, model.Article.FrontpageImage);

            return View(model);
        }

        public ActionResult List() {
            Authorize("Editor", "Journalist");
            ViewBag.User = HalloUser;

            return View(db.Articles.Where(x => x.ArticleType == ArticleTypes.News).OrderByDescending(x => x.Date).Take(15).ToList());
        }

        private Article GetArticle(int id) {
            return db.Articles
                .Where(x => x.Id == id)
                .Include(x => x.Images)
                .Include(x => x.FrontpageImage)
                .Include(x => x.Categories)
                .SingleOrDefault();
        }

        private void PopulateCategories(Article article) {
            List<ArticleCategory> allCategories = db.Categories.OrderBy(x => x.LocalName).ToList();
            List<AssignedArticleCategory> viewModel = new List<AssignedArticleCategory>();
            List<ArticleCategory> selectedCategories = article.Categories.ToList();

            foreach (ArticleCategory c in allCategories) {
                viewModel.Add(new AssignedArticleCategory() {
                    CategoryId = c.Id,
                    Name = c.LocalName,
                    Assigned = selectedCategories.Contains(c)
                });
            }

            ViewBag.Categories = viewModel;
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            Authorize("Editor", "Journalist");

            Article article = GetArticle(id);
            if (article.ArticleType == ArticleTypes.FamilyPresentation)
                return RedirectToAction("Edit", "FamilyPresentation", new { id = id });
            
            PopulateCategories(article);
            return View(article);
        }

        [HttpGet]
        public ActionResult Create() {
            Authorize("Editor", "Journalist");

            Article newArticle = new Article() {
                Date = DateTime.Now, ArticleType = ArticleTypes.News
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

        private void UpdateCategories(Article article, String[] selectedCategories) {
            article.Categories = new List<ArticleCategory>();

            if (selectedCategories == null) {
                return;
            }

            foreach (string id in selectedCategories) {
                int categoryId = int.Parse(id);
                article.Categories.Add(db.Categories.FirstOrDefault(x => x.Id == categoryId));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article article, String[] selectedCategories) {
            Article dbArticle = GetArticle(article.Id);

            dbArticle.Headline = article.Headline;
            dbArticle.Author = article.Author;
            dbArticle.FrontpageText = article.FrontpageText;
            dbArticle.Text = article.Text;

            UpdateCategories(dbArticle, selectedCategories);

            db.SaveChanges();

            PopulateCategories(dbArticle);

            return View(dbArticle);
        }

        public JsonResult SetApproved(int id, bool approved) {
            Article a = GetArticle(id);
            a.ApprovedByEditor = approved;
            a.Date = DateTime.Now;
            db.SaveChanges();
            return Json(null);
        }

        public JsonResult SetPublic(int id, bool isPublic) {
            Article a = GetArticle(id);
            a.IsPublic = isPublic;
            db.SaveChanges();
            return Json(null);
        }
    }
}
