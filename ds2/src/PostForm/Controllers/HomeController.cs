﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PostForm.Models;
using Grpc.Net.Client;
using BackendApi;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PostForm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

 
        [HttpPost]
        public async Task<ActionResult> Create(string JobText)
        {
            
            var config = new ConfigurationBuilder()                    
                .SetBasePath(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName + "/config")  
                .AddJsonFile("config.json", optional: false)
                .Build(); 

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress($"http://localhost:{config.GetValue<int>("Api:port")}");
            var client = new Job.JobClient(channel);
            var reply = await client.RegisterAsync(
                              new RegisterRequest { Description = JobText });
            var text = "Job Id: " + reply.Id + " Job Description: " + JobText;
           return Ok(text);
        } 
    }
}
