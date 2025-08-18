using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockPlaform.Extensions;
using StockPlaform.Services;
using StockPlatform.DTOS.Comments;
using StockPlatform.Helpers;
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
        private readonly IFMPService fmpservice;

        public CommentController(ICommentRepository commentrepo, IStockRepository stockrepo , UserManager<AppUser> UserManager , IFMPService fmpservice)
        {
            this.commentrepo = commentrepo;
            Stockrepo = stockrepo;
            userManager = UserManager;
            this.fmpservice = fmpservice;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]CommentQueryObject QueryObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await commentrepo.GetallAsync(QueryObject);
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

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> Create(string symbol, [FromBody] CreateCommentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // to get all validation errors we provided in dtos
            }

            if (createDto == null)
            {
                return BadRequest("Invalid comment data.");
            }

            var stock = await Stockrepo.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                stock = await fmpservice.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("stock not exist at fmp");
                }

                else
                {
                    // If stock is not found in the database, create a new stock entry

                    //stock.Symbol = symbol;
                    await Stockrepo.CreateAsync(stock);


                }

            }






            //get user from jwt token claims
            var userName = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(userName);
            var newComment = createDto.ToCommentFromCreate(stock.Id);
            newComment.AppUserId = appUser.Id; // set the AppUserId from the authenticated user
            await commentrepo.CreateAsync(newComment);
            return CreatedAtAction(nameof(GetById), new { id = newComment.Id }, newComment.ToCommentDto());
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

