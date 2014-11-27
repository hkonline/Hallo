using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailSender {
    class Program {
        static void Main(string[] args) {
            CultureInfo danish = new CultureInfo("da-DK");
            Thread.CurrentThread.CurrentCulture = danish;
            Thread.CurrentThread.CurrentUICulture = danish;

            Mailer mailer = new Mailer();
            if (mailer.hasMessages)
                mailer.SendMails();
        }
    }
}
