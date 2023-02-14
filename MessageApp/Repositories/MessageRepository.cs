using MessageApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MessageApp.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private static List<Message> messages = new List<Message>();
        //private static string filePath = @"C:\messages.json";
        private static string outputFilePath = @"C:\Users\tayya\Downloads\Compressed";

        private string outPutPath = string.Empty;
        public MessageRepository(IWebHostEnvironment webHostEnvironment)
        {
            outputFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "output.json");
            this.webHostEnvironment = webHostEnvironment;
        }
       
        public async Task AddMessage(Message data)
        {
            //string jsonMessages = JsonConvert.SerializeObject(messages);
            string jsonMessages = string.Empty;

            string readdata = System.IO.File.ReadAllText(outputFilePath);

            List<Message> message1 = JsonConvert.DeserializeObject<List<Message>>(readdata);
             if (message1 == null || message1.Count == 0 || string.IsNullOrEmpty(readdata))
            {
                messages.Add(data);
                jsonMessages = JsonConvert.SerializeObject(messages);
            }
            else
            {
                message1.Add(data);
                jsonMessages = JsonConvert.SerializeObject(message1);
            }
            System.IO.File.WriteAllText(outputFilePath, jsonMessages);
        }

        public IEnumerable<Message> ReceivedMessages(string receiverId, bool isDeleted)
        {
            string jsonMessages = System.IO.File.ReadAllText(outputFilePath);
            messages = JsonConvert.DeserializeObject<List<Message>>(jsonMessages);

            IEnumerable<Message> receiverMessages = messages.Where(m => m.ReceiverID == receiverId).ToList();
            if (isDeleted)
            {
                messages = messages.Where(m => m.ReceiverID != receiverId).ToList();
                jsonMessages = JsonConvert.SerializeObject(messages);
                System.IO.File.WriteAllText(outputFilePath, jsonMessages);
            }
            return receiverMessages;
        }
    }
}
