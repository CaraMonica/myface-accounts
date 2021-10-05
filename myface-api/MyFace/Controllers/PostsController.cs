using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;

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

        public UserAuthentication GetAuthenticationModel(string auth)
        {
            // TODO checks
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(auth.Remove(0, "Base ".Length)));
            var credentialsList = credentials.Split(':');
            return new UserAuthentication(credentialsList[0], credentialsList[1]);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost, [FromHeader(Name = "Authorization")] string auth)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authModel = GetAuthenticationModel(auth);
            var user = _usersRepo.GetByUsername(authModel.UserName);

            if (HashedPasswordGenerator.GenerateHash(authModel.Password, user.Salt) == user.HashedPassword)
            {
                var post = _posts.Create(newPost);
                var url = Url.Action("GetById", new { id = post.Id });
                var postResponse = new PostResponse(post);
                return Created(url, postResponse);
            }

            return BadRequest();
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