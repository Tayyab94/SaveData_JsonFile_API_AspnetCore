using MessageApp.Models;
using MessageApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string Baseurl = "https://localhost:44398";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Index1(string receiverId, bool isTrue)
        {
            using(var c = new HttpClient())
            {
                using (var response = await c.GetAsync($"{Baseurl}/api/Chat?receiverID={receiverId}&purge={isTrue}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var messages = JsonConvert.DeserializeObject<List<Message>>(apiResponse);
                    return Json(new { msgData = messages });
                }
            }

           
            //using (var client = new HttpClient())
            //{


            //    client.BaseAddress = new Uri($"{Baseurl}/api/Messages?{receiverId}=1&purge={isTrue}");
            //    //HTTP GET
            //    var responseTask = client.GetAsync("Message");
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsStringAsync();
            //        readTask.Wait();
            //        var data = JsonConvert.DeserializeObject<List<Message>>(readTask.Result);

            //        if (data.Count == 0)
            //        {
            //            return RedirectToAction("CreateStudent");
            //        }
            //        return Json(new { msgData = data });
            //    }
            //    else //web api sent error response 
            //    {

            //        var data = Enumerable.Empty<Message>();

            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            ////Passing service base url

            return View();
        }


        [HttpGet]
        public IActionResult MessageSender()
        {
            return View(new MessageSenderViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> MessageSender(MessageSenderViewModel model)
        {
            if(ModelState.IsValid==false)
            {
                return View(model);
            }
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{Baseurl}/api/Chat", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("MessageSender");
                }
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
