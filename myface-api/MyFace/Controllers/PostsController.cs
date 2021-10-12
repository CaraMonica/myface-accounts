using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _usersRepo;

        public PostsController(IPostsRepo posts, IUsersRepo usersRepo)
        {
            _posts = posts;
            _usersRepo = usersRepo;
        }


        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
        {
            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
        {
            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        public bool isValidAuthorization(string authHeader)
        {
            var authHeaderRegex = new Regex(@"Basic (.*)");

            if (!authHeaderRegex.IsMatch(authHeader))
                return false;

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Remove(0, "Base ".Length)));
            var credentialsList = credentials.Split(':');
            var authModel = new UserAuthentication(credentialsList[0], credentialsList[1]);
            var userList = _usersRepo.Search(new UserSearchRequest(authModel.UserName)).ToList();

            if (userList.Count == 0 ||
            !(HashedPassword.GenerateHash(authModel.Password, userList[0].Salt) == userList[0].HashedPassword))
                return false;

            return true;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost, [FromHeader(Name = "Authorization")] string authHeader)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = _posts.Create(newPost);
            var url = Url.Action("GetById", new { id = post.Id });
            var postResponse = new PostResponse(post);

            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _posts.Delete(id);
            return Ok();
        }
    }
}