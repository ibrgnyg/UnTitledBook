using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnTitledBook.Application.DTOs
{
    public class DTOUpdateBook
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ShelfLocation { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public bool IsShared { get; set; } = false;
    }
}
