namespace Application.Comments
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
