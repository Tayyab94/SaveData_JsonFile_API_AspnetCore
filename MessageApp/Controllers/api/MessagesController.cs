using MessageApp.Models;
using MessageApp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace MessageApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private static List<Message> messages = new List<Message>();
        //private static string filePath = @"C:\messages.json";
        private static string outputFilePath = @"C:\Users\tayya\Downloads\Compressed";

        private string outPutPath = string.Empty;
        public MessagesController(IWebHostEnvironment webHostEnvironment)
        {


            outputFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "output.json");
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IActionResult Sendmessage([FromBody] MessageSenderViewModel model)
        {
            if (!System.IO.File.Exists(Path.Combine(webHostEnvironment.ContentRootPath, "output.json")))
            {
                System.IO.File.Create(Path.Combine(webHostEnvironment.ContentRootPath, "output.json"));
            }

            Message message = new Message
            {
                SenderID = model.senderID,
                ReceiverID = model.receiverID,
                Msg = model.msg,
                Timestamp = DateTime.Now
            };



            //string jsonMessages = JsonConvert.SerializeObject(messages);
            string jsonMessages = string.Empty;

            // Write data to a JSON file

            //string outputJson = JsonSerializer.Serialize(message);
            //File.WriteAllText(outputFilePath, jsonMessages);

            string readdata = System.IO.File.ReadAllText(outputFilePath);
            List<Message> message1 = JsonConvert.DeserializeObject<List<Message>>(readdata);
            if (message1 == null || message1.Count == 0 || string.IsNullOrEmpty(readdata))
            {
                messages.Add(message);
                jsonMessages = JsonConvert.SerializeObject(messages);
            }
            else
            {
                message1.Add(message);
                jsonMessages = JsonConvert.SerializeObject(message1);
            }
            System.IO.File.WriteAllText(outputFilePath, jsonMessages);
            //if (System.IO.Directory.Exists(filePath) == false)
            //{
            //    System.IO.Directory.CreateDirectory(Path.Combine(filePath));
            //}


            //using FileStream fileStreem = new FileStream(Path.Combine(filePath, "message.json"), FileMode.Create);

            //if (System.IO.Directory.Exists(Path.Combine(webHostEnvironment.WebRootPath, "messagesFolder")) == false)
            //{
            //    System.IO.Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath,"messagesFolder"));
            //}


            //using FileStream fileStreem = new FileStream(Path.Combine(webHostEnvironment.WebRootPath, "messagesFolder"), FileMode.Create);

            //System.IO.File.WriteAllText(Path.Combine(webHostEnvironment.WebRootPath,"messagesFolder"), jsonMessages);


            return Ok();


        }

        //[ResponseType(typeof(IEnumerable<Message>))]
        [HttpGet]
        public IEnumerable<Message> receiveMsg(string receiverID, bool purge)
        {
            //string outputFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "output.json");
            string jsonMessages = System.IO.File.ReadAllText(outputFilePath);
            messages = JsonConvert.DeserializeObject<List<Message>>(jsonMessages);

            IEnumerable<Message> receiverMessages = messages.Where(m => m.ReceiverID == receiverID).ToList();
            if (purge)
            {
                messages = messages.Where(m => m.ReceiverID != receiverID).ToList();
                jsonMessages = JsonConvert.SerializeObject(messages);
                System.IO.File.WriteAllText(outputFilePath, jsonMessages);
            }
            return receiverMessages;
        }
    }
}
