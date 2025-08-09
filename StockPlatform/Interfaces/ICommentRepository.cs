using StockPlatform.Models;

namespace StockPlatform.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comments>> GetallAsync();
        public Task<Comments?> GetByIdAsync(int id);
        public Task<Comments> CreateAsync(Comments comment);
        public Task<Comments?> UpdateAsync( int id , Comments comment);
        public Task<Comments?> DeleteAsync(int id);
    }
}
