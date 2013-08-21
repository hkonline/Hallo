using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using HalloDal.Models.Content;
using System.Collections.Generic;
using HalloDal.Models;

namespace HalloDalTest {
    [TestClass]
    public class UnitTest1 {

        public SqlConnection GetSourceConnection() {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["kobenhavn"].ConnectionString);
            con.Open();
            return con;
        }

        public void GetOldArticles(HalloContext context) {
            SqlConnection sourceConnection = GetSourceConnection();
            SqlCommand articleCmd = new SqlCommand(
                "select top 10 * from hk_artikler oa where oa.artikelID not in (select OldId from hallo.dbo.articles) order by dato desc, ArtikelID desc",
                sourceConnection
            );
            SqlDataReader articleReader = articleCmd.ExecuteReader();

            while (articleReader.Read()) {
                int oldId = int.Parse(articleReader["ArtikelID"].ToString());

                Nullable<int> cat1 = articleReader.GetInt32(articleReader.GetOrdinal("Category"));
                Nullable<int> cat2 = articleReader.GetInt32(articleReader.GetOrdinal("Category2"));

                context.Articles.Add(new Article {
                    Headline = articleReader["overskrift"].ToString(),
                    Author = articleReader["forfatter"].ToString(),
                    Date = (DateTime)articleReader["Dato"],
                    FrontpageText = articleReader["ForsideTekst"].ToString(),
                    FrontpageImage = GetOldFrontPageImage(oldId),
                    Text = articleReader["Tekst"].ToString(),
                    ApprovedByEditor = (bool)articleReader["IsCheckedByJens"],
                    IsPublic = (bool)articleReader["PublicArticle"],
                    ArticleType = articleReader["ArticleType"].ToString().Equals("INFO") ? ArticleTypes.Info : ArticleTypes.Article,
                    Category = context.Categories.FirstOrDefault(x => x.Id == cat1),
                    Category2 = context.Categories.FirstOrDefault(x => x.Id == cat2),
                    Images = GetOldImages(oldId),
                    OldId = oldId
                });
            }
            context.SaveChanges();
            articleReader.Close();
            sourceConnection.Close();
        }

        public ICollection<Image> GetOldImages(int articleId) {
            SqlConnection sourceConnection = GetSourceConnection();
            SqlCommand imageCmd = new SqlCommand(
                "select b.* from HK_BILLEDER b join hk_artikler a on a.ArtikelID = b.ArtikelID AND b.BilledeID <> a.ForsideBilledeID " +
                "where a.ArtikelID = " + articleId + " order by b.OrderNr, b.BilledeId",
                sourceConnection
            );
            SqlDataReader imageReader = imageCmd.ExecuteReader();
            ICollection<Image> images = new List<Image>();
            while (imageReader.Read()) {
                images.Add(new Image {
                    Description = imageReader["Description"].ToString(),
                    OrderNr = imageReader.GetInt32(imageReader.GetOrdinal("OrderNr")),
                    OldId = int.Parse(imageReader[0].ToString())
                });
            }
            imageReader.Close();
            sourceConnection.Close();
            return images;
        }

        public Image GetOldFrontPageImage(int articleId) {
            Image i = null;
            SqlConnection sourceConnection = GetSourceConnection();
            SqlCommand imageCmd = new SqlCommand(
                "SELECT b.* from HK_ARTIKLER a join HK_BILLEDER b on a.ForsideBilledeID = b.BilledeID and a.ArtikelID = " + articleId,
                sourceConnection
            );
            SqlDataReader imageReader = imageCmd.ExecuteReader();
            if (imageReader.Read()) {
                i = new Image {
                    Description = imageReader["Description"].ToString(),
                    OrderNr = imageReader.GetInt32(imageReader.GetOrdinal("OrderNr")),
                    OldId = int.Parse(imageReader[0].ToString())
                };
            }
            imageReader.Close();
            sourceConnection.Close();
            return i;
        }

        public void InitArticleCategories(HalloContext context) {
            if (context.Categories.Count() > 0) return;

            SqlConnection sourceConnection = GetSourceConnection();
            SqlCommand catCmd = new SqlCommand(
                "SELECT * from HK_ARTICLE_CATEGORIES order BY CategoriID",
                sourceConnection
            );
            SqlDataReader catReader = catCmd.ExecuteReader();
            while (catReader.Read()) {
                context.Categories.Add(new ArticleCategory {
                    Name = catReader["CategoriName"].ToString()
                });
            }
            context.SaveChanges();
            catReader.Close();
            sourceConnection.Close();
        }

        [TestMethod]
        public void TestMethod1() {
            HalloContext context = new HalloContext();
            InitArticleCategories(context);
            GetOldArticles(context);
            Assert.IsTrue(context.Categories.Count() > 5);
        }
    }
}
