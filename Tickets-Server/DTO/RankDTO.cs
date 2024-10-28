using System.ComponentModel.DataAnnotations;

namespace Tickets_Server.DTO
{
    public class RankDTO
    {
        public int RankId { get; set; }

        public string RankType { get; set; }

        public RankDTO() { }

    }
}
