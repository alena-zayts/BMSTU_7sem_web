namespace WebApplication1.Models
{
    public class UserAccount
    {
        public UserAccount(string userEmail, string password, string role)
        {
            UserEmail = userEmail;
            Password = password;
            Role = role;
        }

        public string UserEmail { get; }
        public string Password { get; }
        public string Role { get; }

    }
}

