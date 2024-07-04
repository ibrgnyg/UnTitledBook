using UntitledBook.Domain.Enums;

namespace UnTitledBook.Application.DTOs
{
    public class DTOFilter
    {
        public string UserId { get; set; } = string.Empty;
        public string? SearchTerm { get; set; } = string.Empty;
        public FilterDateType FilterDate { get; set; } = FilterDateType.NewAdded;
    }
}
