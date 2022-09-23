using Service.Contracts;
using Microsoft.AspNetCore.Mvc; 

namespace Presentation.Controllers
{
    // [Route("api/post")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IServiceManager _service;
        
        public GameController(IServiceManager service)
        {
            _service = service;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetPosts([FromQuery] PostParameters postParameters)
        // {
        //     var pagedPosts = await _service.PostService.GetAllPostsAsync(postParameters,false);
        //  
        //    var response=   new PostDtoWithPagination{Posts = pagedPosts.posts, Data = pagedPosts.metaData};
        //     
        //   
        //       return Ok(response);
        // }

        // [HttpGet("{id:guid}", Name = "PostById")]
        //
        // public async Task<IActionResult> GetPost(Guid id)
        // {
        //     var post = await _service.PostService.GetPostAsync(id, false);
        //     return Ok(post);
        // }

        // [HttpGet("user")]
        //  [Authorize(Roles = "User")]
        // public async Task<IActionResult> GetPostsByUserName()
        // {
        //     var post = await _service.PostService.GetPostsByUserName(User.Identity.Name, false);
        //     return Ok(post);
        // }
        

        // [HttpPost]
        // [ServiceFilter(typeof(ValidationFilterAttribute))]
        // [Authorize]
        // public async Task<IActionResult> CreatePost([FromBody] PostForCreationDto post)
        // {
        //     var createdPost = await _service.PostService.CreatePostAsync(post, User.Identity.Name);
        //
        //     return CreatedAtAction("CreatePost", new { id = createdPost.Id }, createdPost);
        // }

        // [Authorize]
        // [HttpDelete("{id:guid}")]
        // public async Task<IActionResult> DeletePost(Guid id)
        // {
        //     await _service.PostService.DeletePostAsync(id, false);
        //
        //     return NoContent();
        // }

        // [HttpPut]
        // [ServiceFilter(typeof(ValidationFilterAttribute))]
        // [Authorize]
        // public async Task<IActionResult> UpdatePost( [FromBody] PostForUpdateDto post)
        // {
        //     await _service.PostService.UpdatePostAsync(post.PostId, post, true);
        //
        //     return NoContent();
        // }
    }
}