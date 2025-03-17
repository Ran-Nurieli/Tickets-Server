using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets_Server.DTO;
using Tickets_Server.Models;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketServerApiController : ControllerBase
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

                DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);

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

        [HttpPost("AddTicket")]
        public IActionResult AddTicket([FromBody] DTO.TicketDTO ticketDTO)
        {
            try
            {
                // Convert the DTO to a Ticket model using the constructor that accepts TicketDTO
                Models.Ticket ticket = new Models.Ticket(ticketDTO);

                // Add the ticket to the database
                context.Tickets.Add(ticket);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateUser")]
        public IActionResult UpdateProfile(DTO.UserDTO userDto)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }


                Models.User? user = context.GetUser(userEmail);
                context.ChangeTracker.Clear();
                if (user == null || (user.IsAdmin == false && userDto.Email != user.Email))
                {
                    return Unauthorized("Non Manager User is trying to update a different user");
                }

                Models.User appUser = userDto.GetModels();

                context.Entry(appUser).State = EntityState.Modified;

                context.SaveChanges();


                return Ok();



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("SellTicket")]
        public IActionResult SellTicket(DTO.TicketDTO ticketDto)
        {
            try
            {
                // Retrieve the ticket from the database
                var ticket = context.Tickets
                    .FirstOrDefault(t => t.TicketId == ticketDto.TicketId);

                // If no ticket is found, return a NotFound response
                if (ticket == null)
                {
                    return NotFound(new { message = "Ticket not found" });
                }

                // Validate the price, ensuring it doesn't exceed the original price
                if (ticketDto.Price > ticket.Price)
                {
                    return Unauthorized(new { message = "You can't sell a ticket for more than its original price." });
                }

                // Return a success response
                return Ok(new { message = "Ticket sold successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (or return it for debugging purposes)
                return BadRequest(new { message = "An error occurred while selling the ticket.", error = ex.Message });
            }
        }

        [HttpGet("GetTickets")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                // Assuming tickets are stored in a Tickets table or similar in your DB
                var tickets = await context.Tickets.ToListAsync();

                if (tickets == null || !tickets.Any())
                {
                    return NotFound("No tickets found.");
                }

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        [HttpDelete("RemoveTicket")]
        public async Task<IActionResult> RemoveTicket([FromBody] int ticketId)
        {
            try
            {
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();
                }
                Models.User? modelsUser = context.GetUser(curMail);
                if (modelsUser == null || !modelsUser.IsAdmin)
                {
                    return Unauthorized();
                }


                // Find the ticket in the database matching the row and seat from the request
                var ticketToRemove = await context.Tickets
                    .Where(x => x.TicketId == ticketId)
                    .FirstOrDefaultAsync();

                if (ticketToRemove == null)
                {
                    return NotFound("Ticket not found.");
                }

                // Remove the found ticket
                context.Tickets.Remove(ticketToRemove);
                await context.SaveChangesAsync();

                return Ok("Ticket removed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser([FromBody] string mail)
        {
            try
            {
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();
                }
                Models.User? modelsUser = context.GetUser(curMail);
                if (modelsUser == null || !modelsUser.IsAdmin)
                {
                    return Unauthorized();
                }

                var userToRemove = await context.Users.Where(x => x.Email == mail).FirstOrDefaultAsync();
                if (userToRemove == null)
                {
                    return NotFound("user not found");
                }
                context.Users.Remove(userToRemove);
                await context.SaveChangesAsync();
                return Ok("user removed succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occured: {ex.Message}");
            }
        }



        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();
                }
                Models.User? modelsUser = context.GetUser(curMail);
                if (modelsUser == null || !modelsUser.IsAdmin)
                {
                    return Unauthorized();
                }
                var Users = await context.Users.ToListAsync();
                if (Users == null)
                {
                    return NotFound();
                }
                return Ok(Users);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
