using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class Account
    {
        [Key]
        public long AccountId { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Email { get; set; }
        public byte[] Salt { get; set; }


        public Account(string username, byte[] password, string email, byte[] salt)
        {
            Username = username;
            Password = password;
            Email = email;
            Salt = salt;
        }
    }
}
