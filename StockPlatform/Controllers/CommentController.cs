using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockPlaform.Extensions;
using StockPlatform.DTOS.Comments;
using StockPlatform.Interfaces;
using StockPlatform.Mappers;
using StockPlatform.Models;

namespace StockPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentrepo;
        private readonly IStockRepository Stockrepo;
        private readonly UserManager<AppUser> userManager;

        public CommentController(ICommentRepository commentrepo, IStockRepository stockrepo , UserManager<AppUser> UserManager)
        {
            this.commentrepo = commentrepo;
            Stockrepo = stockrepo;
            userManager = UserManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await commentrepo.GetallAsync();
            var commentDto = comments.Select(c => c.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await commentrepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create(int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await Stockrepo.StockExist(stockId))
            {
                return NotFound("Stock not found");
            }

            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);

            var comment = commentDto.ToCommentFromCreate(stockId);
            comment.AppUserId = appUser.Id;
            await commentrepo.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto updatedto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = updatedto.ToCommentFromUpdate();
            var updatedComment = await commentrepo.UpdateAsync(id, update);

            if (updatedComment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(updatedComment.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deletedComment = await commentrepo.DeleteAsync(id);

            if (deletedComment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(deletedComment.ToCommentDto());
        }
    }
}

