using System.ComponentModel.DataAnnotations;

namespace MyFace.Models
{
    public class UserAuthentication
    {
        public UserAuthentication(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        [Required]
        public string UserName { get; }

        [Required]
        public string Password { get; }
    }
}