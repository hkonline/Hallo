using Hallo.ViewModels;
using HalloDal.Models;
using HalloDal.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hallo.Infrastructure {
    public class MenuBuilder {

        public MenuBuilder() { }

        public List<HKMenuItem> FrontpageMenu(HalloContext context) {
            List<HKMenuItem> menu = new List<HKMenuItem>();

            List<HKMenuItem> categoryLinks = new List<HKMenuItem>();
            List<ArticleCategory> cList = context.Categories.OrderBy(c => c.LocalName).ToList();
            
            foreach (ArticleCategory ac in cList)
                categoryLinks.Add(new HKMenuItem { Text = ac.LocalName, Url = "/Home/Index/" + ac.Id });

            menu.Add(new HKMenuItem() { Text = "Artikler", SubMenu = categoryLinks });
            //menu.Add(new HKMenuItem() { Text = "Kalender", Url = "/Calender/Index" });


            List<HKMenuItem> externalLinks = new List<HKMenuItem>();
            List<FrontpageLink> fpList = context.FrontpageLinks.ToList();
            
            foreach(FrontpageLink fpl in fpList)
                externalLinks.Add(new HKMenuItem { Text = fpl.Label, Url = fpl.NavigationUrl});

            menu.Add(new HKMenuItem() { Text = "Links", SubMenu = externalLinks });

            return menu;
        }

    }
}