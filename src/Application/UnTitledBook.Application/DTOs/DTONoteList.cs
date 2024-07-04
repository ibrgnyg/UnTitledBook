using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledBook.Domain.Enums;

namespace UnTitledBook.Application.DTOs
{
    public class DTONoteList
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public NoteType NoteType { get; set; } = NoteType.Public;
        public DateTime CreatedDate { get; set; }
        public DTOBook Book { get; set; }
        public DTOUser Friend { get; set; }
    }

}
