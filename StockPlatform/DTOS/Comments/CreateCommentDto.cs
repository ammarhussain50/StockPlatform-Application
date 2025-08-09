using System.ComponentModel.DataAnnotations;

namespace StockPlatform.DTOS.Comments
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(10, ErrorMessage = "Content must be at least 10 characters long.")]
        [MaxLength(100, ErrorMessage = "Content cannot exceed 100 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}
