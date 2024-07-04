using Microsoft.AspNetCore.Http;

namespace UnTitledBook.Application.DTOs
{
    public class DTOUserProfile
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
}
