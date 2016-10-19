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

namespace DugnadReminder {
    class Program {
        String connectionString = ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString;
        DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

        DateTime Now { get { return now; } }
        DateTime Today { get { return now.Date; } }

        static String qGetDugnadsForReminder1 = "select dugnadId from hk_dugnader where reminder1 < @date and (reminder1Sent is null or reminder1Sent = 0)";
        static String qGetDugnadsForReminder2 = "select dugnadId from hk_dugnader where reminder2 < @date and (reminder2Sent is null or reminder2Sent = 0)";
        static String qMobilNumbersForDugnad = "SELECT u.mobil FROM HK_DUGNAD_TILMELDING dt join HK_USER u ON u.UserID = dt.UserID where DugnadID = ";
        static String qUpdateDugnadReminder1Sent = "update hk_dugnader set reminder1sent = 1 where DugnadID = ";
        static String qUpdateDugnadReminder2Sent = "update hk_dugnader set reminder2sent = 1 where DugnadID = ";

        private Dugnad GetDetails(String dugnadID) {
            Dugnad dugnad = new Dugnad();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                string query = "select beskrivelse, start from hk_dugnader where dugnadid = " + dugnadID;
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    dugnad.Description = reader[0].ToString();
                    dugnad.Start = reader.GetDateTime(1);
                }
                reader.Close();
                connection.Close();
            }
            return dugnad;
        }

        private List<string> GetList(string query) {
            List<string> list = new List<string>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                if (query.Contains("@date")) cmd.Parameters.AddWithValue("@date", Now);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(reader[0].ToString());
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

            foreach (string s in GetList(qGetDugnadsForReminder1)) {
                Dugnad dugnad = GetDetails(s);
                String dateString = " " + Today.ToLongDateString();
                if (Today == dugnad.Start.Date) dateString = " i dag ";
                if (Today.AddDays(1) == dugnad.Start.Date) dateString = " i morgen ";
                String text = "Husk at du er tilmeldt følgende dugnad: " + dugnad.Description + dateString + " kl. " + dugnad.Start.ToShortTimeString();
                SendSMS(text, GetList(qMobilNumbersForDugnad + s));
                ExecuteNonQuery(qUpdateDugnadReminder1Sent + s);
            }

            foreach (string s in GetList(qGetDugnadsForReminder2)) {
                Dugnad dugnad = GetDetails(s);
                String dateString = Today.ToLongDateString();
                if (Today == dugnad.Start.Date) dateString = " i dag ";
                if (Today.AddDays(1) == dugnad.Start.Date) dateString = " i morgen ";
                String text = "Husk at du er tilmeldt følgende dugnad: " + dugnad.Description + dateString + " kl. " + dugnad.Start.ToShortTimeString();
                SendSMS(text, GetList(qMobilNumbersForDugnad + s));
                ExecuteNonQuery(qUpdateDugnadReminder2Sent + s);
            }
        }

        static void Main(string[] args) {
            Program p = new Program();
            p.sendReminder();
        }

        public void SendSMS(String text, IList<string> smsList) {
            MailMessage mail = new MailMessage();
            Encoding encoding = Encoding.GetEncoding(28591);
            SmtpClient client = new SmtpClient();

            try {
                mail.BodyEncoding = encoding;
                mail.Subject = "dkmservice/894504";
                mail.Body = text;
                foreach (String mobil in smsList) {
                    string tlfnr = mobil;
                    tlfnr = tlfnr.Replace("+45", "").Trim();
                    if (tlfnr.Length == 8) {
                        mail.To.Add("Sms45" + tlfnr + "@coolsmsc.dk");
                    }
                }
                client.Send(mail);
                client.Dispose();
            } catch (Exception e) {
                text += e.Message;
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                String query1 = "select dugnadId from hk_dugnader where reminder1 < GetDate() and (reminder1Sent is null or reminder1Sent = 0)";
                SqlCommand cmd = new SqlCommand(query1, connection);
                connection.Open();
                cmd.CommandText = "insert into smslog values(@userID, @Text, @NumberOfSMS, @SMSTime)";
                cmd.Parameters.AddWithValue("@userID", 0);
                cmd.Parameters.AddWithValue("@Text", text);
                cmd.Parameters.AddWithValue("@NumberOfSMS", smsList.Count);
                cmd.Parameters.AddWithValue("@SMSTime", Now);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
