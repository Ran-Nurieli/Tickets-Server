namespace Tickets_Server.DTO
{
    public class GetTicketsDTO
    {

        public int TicketId { get; set; }

        public int Price { get; set; }

        public int Gate { get; set; }

        public int Row { get; set; }

        public int Seats { get; set; }

       public string HomeTeam { get; set; }
       public string CompetingTeam { get; set; }

        public GetTicketsDTO() { }
        public GetTicketsDTO(int ticketId,int price,int gate,int row,int seats,string homeTeam,string CompetingTeam) 
        {
            this.TicketId = ticketId;
            this.Price = price;
            this.Gate = gate;
            this.Row = row;
            this.Seats = seats;
            this.HomeTeam = homeTeam;
            this.CompetingTeam = CompetingTeam;

        }
    }
}
