using System;

namespace HalloDal.Models.Content {
    public partial class Message {
        public int Id { get; set; }
        public MessageType Type { get; set; }
        public string Headline { get; set; }
        public string Author { get; set; }
        public string Info { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsPublic { get; set; }
    }
}
