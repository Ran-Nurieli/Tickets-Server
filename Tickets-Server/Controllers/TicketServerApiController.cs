﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets_Server.DTO;
using Tickets_Server.Models;
using Azure.Identity;

namespace Tickets_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketServerApiController: ControllerBase
    {
        //a variable to hold a reference to the db context!
        private TicketsServerDBContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public TicketServerApiController(TicketsServerDBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }


    }
}
