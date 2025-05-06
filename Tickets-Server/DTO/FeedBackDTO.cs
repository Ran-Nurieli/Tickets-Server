namespace Tickets_Server.DTO
{
    public class FeedBackDTO
    {
        public int FeedBackId { get; set; }

        public int FeedBackType { get; set; }

        public string Info { get; set; }

        public FeedBackDTO() { }

        public FeedBackDTO(Models.FeedBack feedBack)
        {
            this.FeedBackId = feedBack.FeedBackId;
            this.FeedBackType = (int)feedBack.FeedBackType;
            this.Info = feedBack.Info;
        }
    }
}
