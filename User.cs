namespace Note
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();

        public User() { }

        public User(string name, string lastName, string email, string login, string password)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
        }
    }
}
