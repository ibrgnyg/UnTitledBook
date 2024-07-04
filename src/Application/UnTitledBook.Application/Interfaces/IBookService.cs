using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Application.Interfaces
{
    public interface IBookService
    {
        IList<DTOBookList> GetDTOBookLists(DTOFilter model);
        DTOBook? GetDTOBook(string id);
        IResponseResult AddBook(DTOBook model);
        IResponseResult UpdateBook(DTOBook model);
        IResponseResult DeleteBook(string id);
    }
}
