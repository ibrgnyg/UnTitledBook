using Microsoft.AspNetCore.Http;

namespace UnTitledBook.Application.DTOs
{
    public class DTOBook
    {
        public string? Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string ShelfNumber { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
}
