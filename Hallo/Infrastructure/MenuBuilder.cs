using Hallo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hallo.Infrastructure {
    public class MenuBuilder {

        public MenuBuilder() { }

        public List<HKMenuItem> FrontpageMenu() {
            List<HKMenuItem> menu = new List<HKMenuItem>();

            menu.Add(new HKMenuItem() { Text = "Forsiden", Url = "/Home/Index" });
            menu.Add(new HKMenuItem() { Text = "Kalender", Url = "/Calender/Index" });

            return menu;
        }

    }
}