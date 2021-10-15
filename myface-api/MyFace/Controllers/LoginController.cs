using System;
using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController
    {
        private readonly IUsersRepo _users;

        public LoginController(IUsersRepo users)
        {
            _users = users;
        }

        [HttpGet("")]
        [BasicAuthorisation]
        public LoginResponse GetFeed([FromHeader(Name = "Authorization")] string authHeader)
        {
            var user = AuthorizationHelper.GetUserFromHeader(authHeader, _users);
            return new LoginResponse(user);
        }
    }
}