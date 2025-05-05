using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tickets_Server.DTO;
using Tickets_Server.Models;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Tickets_Server.ModelsHelpers;

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
        [HttpGet("BuyTicket")]
        public async Task<IActionResult> BuyTicket([FromQuery] int ticketId)
        {
            try
            {
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();
                }

                var ticket = await context.Tickets.Include(x => x.UserEmailNavigation).Include(x => x.PurchaseRequest).Where(x => x.TicketId == ticketId).FirstOrDefaultAsync();
                if (ticket == null)
                {
                    return NotFound("ticket not found");
                }
                if (ticket.PurchaseRequest != null)
                {
                    return BadRequest("Ticket already purchased");
                }

                string? phoneNumber = ticket.UserEmailNavigation?.Phone;

                PurchaseRequest purchaseRequest = new PurchaseRequest
                {
                    TicketId = ticket.TicketId,
                    BuyerEmail = curMail,
                    IsAccepted = false
                };
                context.PurchaseRequests.Add(purchaseRequest);
                await context.SaveChangesAsync();

                return Ok(new { message = "send a message to-", phoneNumber });
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
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();
                }


                // Convert the DTO to a Ticket model using the constructor that accepts TicketDTO
                Models.Ticket ticket = TicketModelHelper.TicketDTOToTicket(ticketDTO);
                ticket.UserEmail = curMail;
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
        public async Task<IActionResult> SellTicket(DTO.TicketDTO ticketDto)
        {
            try
            {
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();
                }

                Ticket t = TicketModelHelper.TicketDTOToTicket(ticketDto);
                t.UserEmail = curMail;
                // Add the ticket to the database
                context.Tickets.Add(t);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (or return it for debugging purposes)
                return BadRequest(new { message = "An error occurred while selling the ticket.", error = ex.Message });
            }
        }
        [HttpGet("GetTeams")]
        public async Task<IActionResult> GetTeams()
        {
            try
            {
                var teams = await context.Teams.ToListAsync();
                if (teams == null)
                {
                    return NotFound("No teams found.");
                }
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetTickets")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                // Assuming tickets are stored in a Tickets table or similar in your DB
                var tickets = await context.Tickets.Include(x => x.PurchaseRequest).Include(x => x.Team).Where(x => x.PurchaseRequest == null).ToListAsync();

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

        [HttpGet("GetMyTickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            try
            {
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();

                }
                var tickets = await context.Tickets
                    .Include(x => x.PurchaseRequest)
                    .ThenInclude(x => x.BuyerEmailNavigation)
                    .Where(x => x.UserEmail == curMail).ToListAsync();

                if (tickets == null)
                {
                    return NotFound();
                }
                List<MyTicketsDTO> myTickets = new List<MyTicketsDTO>();
                foreach (var ticket in tickets)
                {
                    myTickets.Add(new MyTicketsDTO(ticket));
                }
                return Ok(myTickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class SetPurchaseParameters
        {
            public int TicketID { get; set; }
            public bool IsAccepted { get; set; }
        }   
        [HttpPost("SetPurchaseStatus")]
        public async Task<IActionResult> SetPurchaseStatus([FromBody] SetPurchaseParameters parameters)
        {
            try
            {
                string curMail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(curMail))
                {
                    return Unauthorized();
                }

                var ticket = await context.Tickets
                    .Include(x => x.PurchaseRequest)
                    .Where(x => x.TicketId == parameters.TicketID && x.UserEmail == curMail && x.PurchaseRequest != null)
                    .FirstOrDefaultAsync();
                if(ticket == null)
                {
                    return NotFound("Ticket not found or not purchased by you.");
                }
                if(ticket.PurchaseRequest.IsAccepted)
                {
                    return BadRequest("Ticket already purchased");
                }

                if (parameters.IsAccepted)
                {
                    ticket.PurchaseRequest.IsAccepted = true;
                }
                else
                {
                    context.PurchaseRequests.Remove(ticket.PurchaseRequest);
                }
                await context.SaveChangesAsync();

                return Ok("Purchase request status updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
