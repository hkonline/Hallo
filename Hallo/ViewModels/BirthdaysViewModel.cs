using HalloDal.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hallo.ViewModels {
    public class BirthdaysViewModel {

        private String dayString;
        public String DayString {
            get {
                if (dayString != null) return dayString;
                else return Day.ToLongDateString();
            }
            set { 
                dayString = value; 
            }
        }
        public DateTime Day { get; set; }
        public List<User> Users { get; set; }
    }
}