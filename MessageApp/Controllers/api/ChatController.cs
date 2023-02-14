using MessageApp.Models;
using MessageApp.Models.ViewModels;
using MessageApp.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MessageApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        public ChatController(IMessageRepository messageRepository)
        {
            this._messageRepository = messageRepository;
        }

        [HttpPost]
        public IActionResult Sendmessage([FromBody] MessageSenderViewModel model)
        {
          
            _messageRepository.AddMessage(new Models.Message() { Msg=model.msg, ReceiverID= model.receiverID, SenderID= model.senderID, Timestamp= DateTime.UtcNow});

            return Ok();

        }
        [HttpGet]
        public async Task<IEnumerable<Models.Message>> receiveMsg(string receiverID, bool purge)
        {
            //string outputFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "output.json");
           var receiverMessages =_messageRepository.ReceivedMessages(receiverID, purge);
            return receiverMessages;
        }
    }
}
