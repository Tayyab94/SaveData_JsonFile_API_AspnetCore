using System.ComponentModel.DataAnnotations;

namespace MessageApp.Models.ViewModels
{
    public class MessageSenderViewModel
    {
        [Required(ErrorMessage ="Send Id Requrid")]
       public string senderID { get; set; }
        [Required(ErrorMessage ="Receiver Id Required")]
        public string receiverID { get; set; }
        [Required(ErrorMessage ="Write Message")]
        public string msg { get; set; }
    }
}
