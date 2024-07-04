using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;

namespace UnTitledBook.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Kitapları listeleme. Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek query parameterleri anlamları:
        /// 
        ///     GET /api/GetBooks
        ///     {        
        ///       "UserId": "Unique User Id zorunlu alan!",
        ///       "SearchTerm": "Aranacak Kitap ismi opsiyonel",
        ///       "FilterDateType": "Enum  varsayılan deger 0 ,yeni eklenenler 1 , eski eklenenler"        
        ///     }
        /// </remarks>
        /// <param name="model"></param>   
        [HttpGet]
        public IList<DTOBookList> GetBooks([FromQuery] DTOFilter model)
        {
            return _bookService.GetDTOBookLists(model);
        }

        /// <summary>
        /// Kitap detay getirme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     GET /api/GetBook
        ///     {        
        ///       "Id": "Unique User Id zorunlu alan!"    
        ///     }
        /// </remarks>
        /// <param name="id">Id parametresi zorunlu</param>   
        [HttpGet("{id}")]
        public DTOBook? GetBook(string id)
        {
            return _bookService.GetDTOBook(id);
        }

        /// <summary>
        /// Kitap ekleme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/addBook
        ///     {        
        ///       "Id": "boş olacak",
        ///       "Name": "Kitap ismi zorunlu",
        ///       "Description": "Kitap açıklaması zorunlu",
        ///       "UserId": "Kullanıcı Id zorunlu",
        ///       "Author": "Yazar  zorunlu",
        ///       "ISBN": "ISBN numarasu zorunlu",
        ///       "ImagePath": "Boş olacak",
        ///       "Room": "Kitap oda numarası zorunlu",
        ///       "Section": "Kitap bölümü numarası zorunlu",
        ///       "ShelfNumber": "Kitap raf numarası zorunlu",
        ///       "File": "Resim dosyası jpg veya png formatları opsiyonel",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Kitap başaryıla eklendi",//Mesaj
        ///         "isError": false, //Hata durumunda true olur
        ///         "responseStatus": 0, //(0)Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="model"></param>   
        [HttpPost("addBook")]
        public IResponseResult AddBook([FromForm] DTOBook model)
        {
            return _bookService.AddBook(model);
        }

        /// <summary>
        /// Kitap güncelleme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     PUT /api/updateBook
        ///     {        
        ///       "Id": "Id zorunlu",
        ///       "Name": "Kitap ismi zorunlu",
        ///       "Description": "Kitap açıklaması zorunlu",
        ///       "UserId": "Kullanıcı Id zorunlu",
        ///       "Author": "Yazar  zorunlu",
        ///       "ISBN": "ISBN numarasu zorunlu",
        ///       "ImagePath": "Boş olacak",
        ///       "Room": "Kitap oda numarası zorunlu",
        ///       "Section": "Kitap bölümü numarası zorunlu",
        ///       "ShelfNumber": "Kitap raf numarası zorunlu",
        ///       "File": "Resim dosyası jpg veya png formatları opsiyonel",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Kitap başaryıla güncellendi.",//Mesaj
        ///         "isError": false, //Hata durumunda true olur
        ///         "responseStatus": 0,//(0),Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="model"></param>   
        [HttpPut("updateBook")]
        public IResponseResult UpdateBook([FromForm] DTOBook model)
        {
            return _bookService.UpdateBook(model);
        }

        /// <summary>
        /// Kitap silme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     DELETE /api/deleteBook
        ///     {        
        ///       "Id": "Id zorunlu",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Kitap başaryıla silindi",//Mesaj
        ///         "isError": false, //Hata durumunda true olur
        ///         "responseStatus": 0,//(0),Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="id"></param>  
        [HttpDelete("deleteBook/{id}")]
        public IResponseResult DeleteBook(string id)
        {
            return _bookService.DeleteBook(id);
        }
    }
}
