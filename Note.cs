namespace Note
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateOnly Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public Note() { }

        public Note(string title, string content, DateOnly date, int userId, User user)
        {
            Title = title;
            Content = content;
            Date = date;
            User = user;
        }
    }
}
