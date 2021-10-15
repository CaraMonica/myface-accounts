using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;
using MyFace.Models.Request;

namespace MyFace.Models.Response
{
    public class LoginResponse
    {

        private readonly User _user;

        public LoginResponse(User user)
        {
            _user = user;
        }
        
        public int Id => _user.Id;

    }
}