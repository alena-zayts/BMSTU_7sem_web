namespace ProjectReactRedux.Models
{
    public class UserAccount
    {
        public UserAccount(string userEmail, string password, string role, uint cardID)
        {
            UserEmail = userEmail;
            Password = password;
            Role = role;
            CardID = cardID;
        }


        public uint UserID { get; set; }
        public string UserEmail { get; }
        public string Password { get; }
        public string Role { get; }
        public uint CardID { get; }

    }
}

