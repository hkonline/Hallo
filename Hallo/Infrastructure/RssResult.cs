using Hallo.ViewModels;
using HalloDal.Models;
using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Hallo.Infrastructure {
    public class RssResult : FileResult {

        private readonly HalloContext halloContext;
        private List<Article> articles;
        private readonly String title;
        private Uri currentUrl;
        private int maxArticles = 5;
        private DateTime updatedMin;
        private readonly DateTime minAllowableDate = new DateTime(1900, 1, 1);

        public RssResult() : base("application/rss+xml") { }

        public RssResult(HalloContext context, string title)
            : this() {
            this.halloContext = context;
            this.title = title;
        }

        public override void ExecuteResult(ControllerContext context) {
            currentUrl = context.RequestContext.HttpContext.Request.Url;
            String maxResults = context.RequestContext.HttpContext.Request["max-results"];
            String qUpdatedMin = context.RequestContext.HttpContext.Request["updated-min"];

            if (!String.IsNullOrEmpty(maxResults)) maxArticles = int.Parse(maxResults);
            if (!DateTime.TryParse(qUpdatedMin, out updatedMin) || updatedMin < minAllowableDate) updatedMin = minAllowableDate;

            articles = halloContext.Articles
                .Where(x => x.Date >= updatedMin)
                .Where(x => x.IsPublic == true)
                .OrderByDescending(x => x.Date).Take(maxArticles).ToList();

            base.ExecuteResult(context);
        }

        protected override void WriteFile(HttpResponseBase response) {
            var items = new List<SyndicationItem>();

            foreach (Article article in articles) {
                var item = new SyndicationItem(article.Headline, null, new Uri("http://" + currentUrl.Host + "/Article/Article/" + article.Id)) {
                    Summary = new TextSyndicationContent(article.FrontpageText)
                };
                item.Categories.Add(new SyndicationCategory("News"));
                foreach (var category in article.Categories) {
                    item.Categories.Add(new SyndicationCategory(category.Name));
                }

                item.PublishDate = article.Date;

                item.Id = currentUrl.Host + "-" + article.Id;

                Image image = article.FrontpageImage;
                if (image != null) {
                    ImageViewModel ivm = new ImageViewModel(article.Id, image);
                    item.ElementExtensions.Add(new XElement("enclosure", new XAttribute("url", ivm.GetThumbUrl())));
                }

                items.Add(item);
            }

            SyndicationFeed feed = new SyndicationFeed(title, title, currentUrl, items.AsEnumerable());
            Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(response.Output, settings)) {
                formatter.WriteTo(writer);
            }
        }

    }
}