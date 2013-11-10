using System;

namespace HalloDal.Models.Users {
    public class SmsLog {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public int NumberOfSms { get; set; }
        public DateTime SmsTime { get; set; }
    }
}
