using System;
using System.Text;
using System.Text.RegularExpressions;
using MyFace.Models.Database;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Helpers
{
    public class AuthorizationHelper
    {
        public static User GetUserFromHeader(string authorizationHeader, IUsersRepo usersRepo)
        {
            var authHeaderRegex = new Regex(@"Basic (.*)");

            if (!authHeaderRegex.IsMatch(authorizationHeader))
                throw new ArgumentException("Authorization code not formatted properly.");

            var authBase64 =
            Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    authHeaderRegex.Replace(authorizationHeader, "$1")));

            var authSplit = authBase64.Split(Convert.ToChar(':'), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            var user = usersRepo.GetByUsername(authUsername);

            if (user == null)
                throw new ArgumentException("Invalid username.");

            if (!(HashedPasswordGenerator.GenerateHash(authPassword, user.Salt) == user.HashedPassword))
                throw new ArgumentException("Invalid password.");

            return user;
        }
    }
}