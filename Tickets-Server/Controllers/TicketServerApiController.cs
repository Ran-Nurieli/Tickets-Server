using Microsoft.AspNetCore.Http;
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
        
        private TicketsServerDBContext context;   //a variable to hold a reference to the db context!
        private IWebHostEnvironment webHostEnvironment;          //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)

        public TicketServerApiController(TicketsServerDBContext context, IWebHostEnvironment env) //Use dependency injection to get the db context and web host into the constructor
        {
            this.context = context;
            this.webHostEnvironment = env;
        }




        [HttpPost("login")]
        public IActionResult Login([FromBody] DTO.LoginInfoDTO loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User? modelsUser = context.GetUser(loginDto.Email);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsUser == null || modelsUser.Password != loginDto.Password)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsUser.Email);

                DTO.AppUser dtoUser = new DTO.AppUser(modelsUser);
                dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("register")]
        public IActionResult Register([FromBody] DTO.UserDTO userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt    
                Models.User modelsUser = userDto.GetModels();  //Create model user class
                context.Users.Add(modelsUser);
                context.SaveChanges();                
                DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);  //User was added!
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }



}
