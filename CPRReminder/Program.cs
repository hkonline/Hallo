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

namespace CPRReminder {

    class Person {
        public string mobil;
        public string email;
    }

    class Program {
        String connectionString = ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString;

        private List<Person> GetList() {
            List<Person> list = new List<Person>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                SqlCommand cmd = new SqlCommand("select * from cpr_v where CPR is null or len(CPR)< 10", connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
//                    if (!reader.IsDBNull(1) && reader[1].ToString().Length == 10) {
                    if (reader.IsDBNull(1) || reader[1].ToString().Length < 10) {
                        list.Add(new Person {
                            mobil = reader.IsDBNull(3) ? null : reader[3].ToString(),
                            email = reader.IsDBNull(2) ? null : reader[2].ToString()
                        });
                    }
                }
                reader.Close();
                connection.Close();
            }
            return list;
        }

        private void ExecuteNonQuery(string query) {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void sendReminder() {
            CultureInfo danish = new CultureInfo("da-DK");
            Thread.CurrentThread.CurrentCulture = danish;
            Thread.CurrentThread.CurrentUICulture = danish;


            foreach (Person s in GetList()) {
                String text = @"I forbindelse med indhentning af børne-attest på alle som arbejder med børn og unge i Menigheden i København, har vi brug for at du angiver dit CPR-nr.<br/><br/>Følg venligst denne <a href='http://kobenhavn.brunstad.org/aktivitetsklubben/cpr.aspx'>link</a> for at komme til en side på HK Online, hvor du kan aflevere oplysningen.<br/><br/>Venlig hilsen<br/><br/>T.A.S.K.";
                //SendMail(text, s.email);
                Console.WriteLine("Sendt til: " + s.email);
            }
        }

        static void Main(string[] args) {
            Program p = new Program();
            p.sendReminder();
        }

        //    public void SendSMS(String text, IList<string> smsList) {
        //        MailMessage mail = new MailMessage();
        //        Encoding encoding = Encoding.GetEncoding(28591);
        //        SmtpClient client = new SmtpClient();

        //        try {
        //            mail.BodyEncoding = encoding;
        //            mail.Subject = "dkmservice/894504";
        //            mail.Body = text;
        //            foreach (String mobil in smsList) {
        //                string tlfnr = mobil;
        //                tlfnr = tlfnr.Replace("+45", "").Trim();
        //                if (tlfnr.Length == 8) {
        //                    mail.To.Add("Sms45" + tlfnr + "@coolsmsc.dk");
        //                }
        //            }
        //            client.Send(mail);
        //            client.Dispose();
        //        } catch (Exception e) {
        //            text += e.Message;
        //        }

        //        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
        //            String query1 = "select dugnadId from hk_dugnader where reminder1 < GetDate() and (reminder1Sent is null or reminder1Sent = 0)";
        //            SqlCommand cmd = new SqlCommand(query1, connection);
        //            connection.Open();
        //            cmd.CommandText = "insert into smslog values(@userID, @Text, @NumberOfSMS, @SMSTime)";
        //            cmd.Parameters.AddWithValue("@userID", 0);
        //            cmd.Parameters.AddWithValue("@Text", text);
        //            cmd.Parameters.AddWithValue("@NumberOfSMS", smsList.Count);
        //            cmd.Parameters.AddWithValue("@SMSTime", Now);
        //            cmd.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //    }
        //}

        public void SendMail(String text, string email) {
            MailMessage mail = new MailMessage();
            Encoding encoding = Encoding.GetEncoding(28591);
            SmtpClient client = new SmtpClient();

            try {
                mail.IsBodyHtml = true;
                mail.BodyEncoding = encoding;
                mail.Subject = "2. Reminder: Børneattest";
                mail.Body = text;
                mail.To.Add(email);
                mail.From = new MailAddress("webmaster@hkonline.dk");
                client.Send(mail);
                client.Dispose();
            } catch (Exception exp) {
                string hest = "test";


                int j = 60;
            }
        }
    }
}
