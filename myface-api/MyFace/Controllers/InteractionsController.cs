using System;
using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/interactions")]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionsRepo _interactions;
        private readonly IUsersRepo _usersRepo;

        private readonly IPostsRepo _postsRepo;

        public InteractionsController(IInteractionsRepo interactions, IUsersRepo usersRepo, IPostsRepo postsRepo)
        {
            _usersRepo = usersRepo;
            _interactions = interactions;
            _postsRepo = postsRepo;
        }

        [HttpGet("")]
        public ActionResult<ListResponse<InteractionResponse>> Search([FromQuery] SearchRequest search)
        {
            var interactions = _interactions.Search(search);
            var interactionCount = _interactions.Count(search);
            return InteractionListResponse.Create(search, interactions, interactionCount);
        }

        [HttpGet("{id}")]
        public ActionResult<InteractionResponse> GetById([FromRoute] int id)
        {
            var interaction = _interactions.GetById(id);
            return new InteractionResponse(interaction);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateInteractionRequest interactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _interactions.DeleteByPostId(interactionRequest.PostId, interactionRequest.UserId);
            var interaction = _interactions.Create(interactionRequest);

            var url = Url.Action("GetById", new { id = interaction.Id });
            var post = _postsRepo.GetById(interaction.PostId);
            return Created(url, new FeedPostModel(post));
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromBody] CreateInteractionRequest interactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _interactions.DeleteByPostId(interactionRequest.PostId, interactionRequest.UserId);
            var post = _postsRepo.GetById(interactionRequest.PostId);
            return Ok(new FeedPostModel(post));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _interactions.Delete(id);
            return Ok();
        }
    }
}
