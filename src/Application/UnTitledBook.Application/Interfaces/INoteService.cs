using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Application.Interfaces
{
    public interface INoteService
    {
        List<DTONoteList> SharedNotes(string userId);
        List<DTONoteList> GetNotesBook(string bookId);
        IResponseResult AddNote(DTONote model);
        IResponseResult UpdateNote(string id, DTONote model);
        IResponseResult DeleteNote(string id);
    }
}
