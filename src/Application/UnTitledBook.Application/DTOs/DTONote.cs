using UntitledBook.Domain.Enums;

namespace UnTitledBook.Application.DTOs
{
    public class DTONote
    {
        public string Content { get; set; } = string.Empty;
        public NoteType NoteType { get; set; } = NoteType.Public;
        public string BookId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public List<string>? FriendIds { get; set; } = new();
    }
}
