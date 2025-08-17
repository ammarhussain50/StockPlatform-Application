using StockPlatform.DTOS.Comments;
using StockPlatform.Models;

namespace StockPlatform.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comments comment)
        {
            if (comment == null)
            {
                return null;
            }
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                createdBy = comment.AppUser?.UserName,
                StockId = comment.StockId
            };
        }

        public static Comments ToCommentFromCreate(this CreateCommentDto commentDto ,int stockId )
        {
            return new Comments
            {
               Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId,
            };
        }

        public static Comments ToCommentFromUpdate(this UpdateCommentDto commentDto)
        {
            return new Comments
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
            };
        }
    }
}
