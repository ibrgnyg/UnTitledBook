using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnTitledBook.Application.DTOs
{
    public class DTOUser
    {
        public string Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
