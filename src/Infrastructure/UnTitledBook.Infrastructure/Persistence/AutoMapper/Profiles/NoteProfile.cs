using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledBook.Domain.Entities;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Infrastructure.Persistence.AutoMapper.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<DTONote, Note>();
            CreateMap<Note, DTONote>();

            CreateMap<DTONoteList, Note>();
            CreateMap<Note, DTONoteList>();

        }
    }
}
