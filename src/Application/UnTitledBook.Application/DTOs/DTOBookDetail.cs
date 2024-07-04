using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnTitledBook.Application.DTOs
{
    public class DTOBookDetail
    {
        public string Title { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ShelfLocation { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public bool IsShared { get; set; } = false;
        public string Room { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string ShelfNumber { get; set; } = string.Empty;
        public string ShelfPosition { get; set; } = string.Empty;
    }
}
