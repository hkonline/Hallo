using Hallo.ViewModels;
using HalloDal.Models;
using HalloDal.Models.Content;
using HalloDal.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hallo.Infrastructure {
    public class MenuBuilder {

        public MenuBuilder() { }

        public List<HKMenuItem> FrontpageMenu(HalloContext context, HttpRequestBase request, User user) {
            List<HKMenuItem> menu = new List<HKMenuItem>();

            List<HKMenuItem> categoryLinks = new List<HKMenuItem>();
            List<ArticleCategory> cList = context.Categories.OrderBy(c => c.LocalName).ToList();

            foreach (ArticleCategory ac in cList)
                categoryLinks.Add(new HKMenuItem { Text = ac.LocalName, Url = "/Home/Index/" + ac.Id });

            menu.Add(new HKMenuItem() { 
                Text = "Artikler", 
                SubMenu = categoryLinks , 
                Active = request.FilePath.Contains("Home/Index/") && request.FilePath.Length>11
            });
            if (user.Authorized)
                menu.Add(new HKMenuItem() { Text = "Streaming", Url = "/Meeting/Streaming" });
            
            //menu.Add(new HKMenuItem() { Text = "Kalender", Url = "/Calender/Index" });

            List<HKMenuItem> externalLinks = new List<HKMenuItem>();
            List<FrontpageLink> fpList = context.FrontpageLinks.ToList();

            foreach (FrontpageLink fpl in fpList)
                externalLinks.Add(new HKMenuItem { 
                    Text = fpl.Label, Url = 
                    fpl.NavigationUrl 
                });

            bool linksActive = true;
            foreach(HKMenuItem item in menu) if (item.Active) linksActive = false;

            menu.Add(new HKMenuItem() { 
                Text = "Links", 
                SubMenu = externalLinks,
                Active = linksActive
            });

            return menu;
        }

    }
}