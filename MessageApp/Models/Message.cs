using System;

namespace MessageApp.Models
{
    public class Message
    {
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string Msg { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
