using System;
using System.Collections.Generic;
using System.Linq;
using HalloDal.Models.Users;

namespace Hallo.ViewModels {
    public class MailViewModel {

        public MailViewModel() {
            SmsList = new List<User>();
            EmailList = new List<User>();
        }
       
        public IList<User> SmsList { get; set; }
        public IList<User> EmailList { get; set; }

        public int UserGroupId { get; set; }
        public string Text { get; set; }
        public string Headline { get; set; }
        public bool SendSms { get; set; }
        public bool SendMail { get; set; }
        public bool CurrentUserIsSender { get; set; }

    }
}