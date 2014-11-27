using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailSender {
    class Mailer {
        const int numberOfMailsPrBatch = 50;
        String connectionString = ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString;
        private int messageID;
        Message message;
        public bool hasMessages;

        public Mailer() {
            ExecuteNonQuery("insert into MailerAlive values(Getdate());");
            hasMessages = CheckMessages();
            if (hasMessages) ReadMessage();
        }
                
        private bool CheckMessages() {
            string checkSql = "select count(*), min(MessageId) from MessageQueue";
            int numberOfWaitingMessages = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                SqlCommand cmd = new SqlCommand(checkSql, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    numberOfWaitingMessages = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        messageID = reader.GetInt32(1);
                }
                reader.Close();
                connection.Close();
            }
            return numberOfWaitingMessages > 0;
        }

        private void ReadMessage() {
            string getMessageSql = "select * from MessageLog where messageId = " + messageID;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                SqlCommand cmd = new SqlCommand(getMessageSql, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    message = new Message {
                        Body = (String)reader["Text"],
                        Subject = (String)reader["Subject"],
                        ID = messageID,
                        Sender = reader.IsDBNull(reader.GetOrdinal("Sender")) ? null : (String)reader["Sender"]
                    };
                }
                reader.Close();
                connection.Close();
            }
        }

        public void SendMails() {
            List<string> receivers = GetList();

            MailMessage mail = new MailMessage();
            Encoding encoding = Encoding.GetEncoding(28591);
            SmtpClient client = new SmtpClient();
            bool succes = false;
            try {
                mail.BodyEncoding = encoding;
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = message.Body.Contains("<") && message.Body.Contains(">");
                if (message.Sender != null) mail.From = new MailAddress(message.Sender);
                foreach (String recipient in receivers) {
                    mail.Bcc.Add(recipient.Trim());
                }
                client.Send(mail);
                client.Dispose();
                succes = true;
            } catch (Exception e) {
                ExecuteNonQuery("Insert into MessageExceptions values( " + messageID + ", '" + e.Message + "', '" + e.StackTrace + "')");
            }
            if (succes) RemoveFromQueue(receivers);
        }

        private List<string> GetList() {
            string query = "select top (" + numberOfMailsPrBatch + ") recipient from MessageQueue where MessageId = " + messageID;
            List<string> list = new List<string>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(reader[0].ToString());
                reader.Close();
                connection.Close();
            }
            return list;
        }

        private void RemoveFromQueue(List<String> list) {
            foreach (String r in list)
                ExecuteNonQuery("delete from messageQueue where MessageID = " + messageID + " and Recipient = '" + r + "';");
        }

        private void ExecuteNonQuery(string query) {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HalloDB"].ConnectionString)) {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
