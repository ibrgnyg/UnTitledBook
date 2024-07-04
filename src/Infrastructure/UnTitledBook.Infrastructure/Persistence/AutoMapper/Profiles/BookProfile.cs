using AutoMapper;
using UntitledBook.Domain.Entities;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Infrastructure.Persistence.AutoMapper.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, DTOBookList>();
            CreateMap<DTOBookList, Book>();

            CreateMap<Book, DTOBook>();
            CreateMap<DTOBook, Book>();
        }
    }
}
