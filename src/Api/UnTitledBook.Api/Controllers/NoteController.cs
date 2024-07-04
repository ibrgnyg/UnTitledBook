using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;

namespace UnTitledBook.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// Kitap notlarını listeleme. Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek query parameterleri anlamları:
        /// 
        ///     GET /api/noteBooks
        ///     {        
        ///       "BookId": "Unique Kitap Id zorunlu alan!",
        ///     }
        /// </remarks>
        /// <param name="bookId"></param>  
        [HttpGet("noteBooks")]
        public List<DTONoteList> NotesBook([FromQuery] string bookId)
        {
            return _noteService.GetNotesBook(bookId);
        }

        /// <summary>
        /// Paylaşılan kitap notlarını listeleme arkdaşlar ve herkese açık olan kitap notları listelenir. Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek query parameterleri anlamları:
        /// 
        ///     GET /api/sharedNotes
        ///     {        
        ///       "UserId": "Unique User Id zorunlu alan!",
        ///     }
        /// </remarks>
        /// <param name="userId"></param> 
        [HttpGet("sharedNotes")]
        public List<DTONoteList> GetSharedNotes(string userId)
        {
            return _noteService.SharedNotes(userId);
        }

        /// <summary>
        /// Kitap not ekleme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/addNote
        ///     {        
        ///       "Content": "Not kitap notu zorunlu alan!",
        ///       "NoteType": "Note tipi varsayılan (2) public olarak ekler (0) özel  (1) sadece arkadaşlar (2) public",
        ///       "BookId": "Kitap Id zorunlu alan!",
        ///       "UserId": "UserId zorunlu alan!",
        ///       "FriendIds": "Arakdaş ekleyecek olan UserIdler çoklu opsiyonel! Note UserId Id arkdaşlar listesinde ekli olmalıdır",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Arkadaş başaryıla eklendi",
        ///         "isError": false, //hatalı olma durumunda true
        ///         "responseStatus": 0, //(0)Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        [HttpPost("addNote")]
        public IResponseResult AddNote([FromBody] DTONote model)
        {
            return _noteService.AddNote(model);
        }

        /// <summary>
        /// Kitap not güncelleme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     PUT /api/addNote
        ///     {        
        ///       "Id": "Not Id zorunlu alan ",
        ///       "Content": "Note zorunlu alan ",
        ///       "NoteType": "Note tipi varsayılan (2) public olarak ekler (0) özel  (1) sadece arkadaşlar (2) public",
        ///       "BookId": "Kitap Id zorunlu alan!",
        ///       "UserId": "UserId zorunlu alan!",
        ///       "FriendIds": "Arakdaş ekleyecek olan UserId'ler çoklu opsiyonel! Note UserId Id arkdaşlar listesinde ekli olmalıdır",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Arkadaş başaryıla eklendi",
        ///         "isError": false, // Hata durumunda true olur
        ///         "responseStatus": 0, //(0)Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        [HttpPut("updateNote")]
        public IResponseResult UpdateNote([FromQuery] string id, [FromBody] DTONote model)
        {
            return _noteService.UpdateNote(id, model);
        }

        /// <summary>
        /// Arkadaş listesinde arkadaş silme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     DELETE /api/deleteNote
        ///     {        
        ///       "Id": "Not Id zorunlu",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Note başaryıla silindi", //Mesaj
        ///         "isError": false, // Hata durumunda true olur
        ///         "responseStatus": 0, //(0)Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="id"></param>  
        [HttpDelete("deleteNote/{id}")]
        public IResponseResult DeleteNote(string id)
        {
            return _noteService.DeleteNote(id);
        }
    }
}
