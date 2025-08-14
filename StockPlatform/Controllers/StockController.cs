using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPlatform.Data;
using StockPlatform.DTOS.Stock;
using StockPlatform.Helpers;
using StockPlatform.Interfaces;
using StockPlatform.Mappers;

namespace StockPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        public readonly IStockRepository Stockrepo;

        public StockController(ApplicationDBContext context, IStockRepository stockrepo)
        {
            this.context = context;
            Stockrepo = stockrepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Getall([FromQuery] QueryObject quey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stocks = await Stockrepo.GetAllAsync(quey);
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();
            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await Stockrepo.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto == null)
            {
                return BadRequest("Invalid stock data.");
            }

            var stock = dto.ToStockFromCreateDto();
            await Stockrepo.CreateAsync(stock);

            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFromCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto == null)
            {
                return BadRequest("Invalid stock data.");
            }

            var stock = await Stockrepo.UpdateAsync(id, dto);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await Stockrepo.DeleteAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return NoContent(); // 204 No Content
        }
    }
}
