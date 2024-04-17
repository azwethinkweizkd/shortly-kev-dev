namespace Shortly.Client.Data.ViewModels
{
    public class GetUrlVm
    {
        public int Id { get; set; }
        public string OriginalLink { get; set; }
        public string ShortLink { get; set; }
        public int NumOfClicks { get; set; }
        public string? UserId { get; set; }
        public GetUserVm? User { get; set; }
    }
}
