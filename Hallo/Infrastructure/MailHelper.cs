using HalloDal.Models.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Hallo.Infrastructure {
    public class MailHelper {

        private static readonly Regex emailPattern = new Regex(
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
        );

        public static IList<User> SendSms(String text, IList<User> smsList, String senderPhone) {
            IList<User> sendUsers = new List<User>();

            SmtpClient client = new SmtpClient();

            MailMessage mail = new MailMessage {
                BodyEncoding = Encoding.GetEncoding(28591),
                Subject = ConfigurationManager.AppSettings["CoolSmsSubject"],
                Body = text
            };

            if (!String.IsNullOrEmpty(senderPhone))
                mail.Subject += "/0/" + senderPhone;

            foreach (User user in smsList) {
                string tlfnr = user.MobilPhone;
                if (!String.IsNullOrEmpty(user.MobilPhone)) {
                    tlfnr = tlfnr.Replace("+45", "").Replace(" ", "");
                    if (tlfnr.Length == 8) {
                        mail.To.Add("Sms45" + tlfnr + "@coolsmsc.dk");
                        sendUsers.Add(user);
                    }
                }
            }

            if (mail.To.Count > 0) client.Send(mail);

            return sendUsers;
        }

        public static IList<User> SendMail(String text, String headLine, IList<User> emailList, User sendingUser, bool userIsSender) {
            IList<User> sendUsers = new List<User>();

            SmtpClient client = new SmtpClient();
            MailMessage mail = new MailMessage {
                Subject = headLine,
                Body = text,
                IsBodyHtml = false
            };

            if (userIsSender && emailPattern.IsMatch(sendingUser.Email.Trim()))
                mail.From = new MailAddress(sendingUser.Email.Trim());
            else {
                mail.From = new MailAddress(ConfigurationManager.AppSettings["WebmasterMail"]);
            }

            foreach (var user in emailList) {
                if (!String.IsNullOrEmpty(user.Email.Trim()) && emailPattern.IsMatch(user.Email.Trim())) {
                    mail.Bcc.Add(new MailAddress(user.Email.Trim()));
                    sendUsers.Add(user);
                }
            }

            client.Send(mail);

            return sendUsers;
        }
        /*
            SqlCommand cmd = getCommand();
            cmd.CommandText = "insert into smslog values(@userID, @Text, @NumberOfSMS, @SMSTime)";
            cmd.Parameters.AddWithValue("@userID", user.UserID);
            cmd.Parameters.AddWithValue("@Text", text);
            cmd.Parameters.AddWithValue("@NumberOfSMS", smsList.Count);
            cmd.Parameters.AddWithValue("@SMSTime", DateTime.Now);
            cmd.ExecuteNonQuery();
            returnCommand(cmd);

            messageLabel.Text += "<br />SMS er sendt!";
         */

    }
}