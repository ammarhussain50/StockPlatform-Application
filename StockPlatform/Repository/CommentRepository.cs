using Microsoft.EntityFrameworkCore;
using StockPlatform.Data;
using StockPlatform.Interfaces;
using StockPlatform.Models;

namespace StockPlatform.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext context;

        public CommentRepository(ApplicationDBContext Context)
        {
            context = Context;
        }

        public async Task<Comments> CreateAsync(Comments comment)
        {
            await context.comments.AddAsync(comment);
            await context.SaveChangesAsync();
            return comment;
        }

        public Task<Comments?> DeleteAsync(int id)
        {
            var comment = context.comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return (null);
            }
            context.comments.Remove(comment.Result);
            context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comments>> GetallAsync()
        {
            return await context.comments.Include(a=> a.AppUser).ToListAsync();
        }

        public Task<Comments?> GetByIdAsync(int id)
        {
            return context.comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comments?> UpdateAsync(int id, Comments comment)
        {
            var existingcomment = await context.comments.FirstOrDefaultAsync(c => c.Id == id);
            if (existingcomment != null)
            {
                existingcomment.Content = comment.Content;
                existingcomment.Title = comment.Title;
                await context.SaveChangesAsync();
               
            }
            return existingcomment;

        }
    }
}
