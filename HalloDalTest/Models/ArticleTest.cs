using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HalloDal.Models.Content;
using HalloDal.Models;
using System.Collections.Generic;

namespace HalloDalTest.Models {
    [TestClass]
    public class ArticleTest {
        [TestMethod]
        public void TestSelect() {
            HalloContext context = new HalloContext();

            Article a = context.Articles
                .Where(x => x.ApprovedByEditor == true)
                .FirstOrDefault(x=> x.Id ==2);

            Assert.IsTrue(a.Images.Count > 0);
            Assert.IsTrue(a.FrontpageImage != null);
        }
    }
}
