using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UntitledBook.Domain.Entities;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;

namespace UnTitledBook.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManagerService _userManagerService;

        public UserController(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        /// <summary>
        /// Kullanıcı üye olma. 
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/register
        ///     {        
        ///       "Email": "E-posta Unique olmalıdır zorunlu alan!",
        ///       "UserName": "Unique olmalıdır zorunlu alan!",
        ///       "Password": "Şifre 123456 olabilir zorunlu alan!",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Üye olundu şimdi giriş yapabilirsin", //Mesaj
        ///         "isError": false, // Hata durumunda true olur
        ///         "responseStatus": 0, //(0)Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        [HttpPost("register")]
        public async ValueTask<IResponseResult> RegisterUser([FromBody] DTOUserRegister model)
        {
            return await _userManagerService.RegisterUserAsync(model);
        }

        /// <summary>
        /// Kullanıcı giriş işlemi.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/login
        ///     {
        ///         "Email": "E-posta (zorunlu alan)",
        ///         "Password": "Şifre (zorunlu alan)"
        ///     }
        /// 
        /// Başarılı istek sonucu:
        /// 
        /// {
        ///     "message": "Başarılı"//mesaj,
        ///     "isError": false, // Hata durumunda true olabilir
        ///     "responseStatus": 0, //(0),Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     "fields": {
        ///         "UserId": "1a7267f6-6c1f-4b76-a0c0-19a07750cb35", //user Id
        ///         "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjFhNzI2N2Y2LTZjMWYtNGI3Ni1hMGMwLTE5YTA3NzUwY2IzNSIsImVtYWlsIjoibEBsLmNvbSIsIm5iZiI6MTcyMDEwNTM5MiwiZXhwIjoxNzIwMTkxNzkyLCJpYXQiOjE3MjAxMDUzOTIsImlzcyI6InVudGl0bGVkYm9vay5pc3N1ZXIiLCJhdWQiOiJ1bnRpdGxlZGJvb2suYXVkaWVuY2UifQ.Twn7sr4AmZQe7g-jkmKUmmHwBC1Dpa6Iw1zM-pKAxCs" //token
        ///     }
        ///  }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        [HttpPost("login")]
        public async ValueTask<IResponseResult> LoginUser([FromBody] DTOUserLogin model)
        {
            return await _userManagerService.LoginUserAsync(model);
        }

        /// <summary>
        /// Kullanıcı giriş işlemi. Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     PUT /api/updateProfile
        ///     {
        ///         "UserId": "UserId Kullanıcı Id zorunlu alan!",
        ///         "UserName": "Kullanıcı ismi  zorunlu alan!"
        ///         "File": "Resim dosyası jpg veya png formatları opsiyonel",
        ///     }
        /// 
        /// Başarılı istek sonucu:
        /// 
        /// {
        ///     "message": "Profil bilgisi güncellendi",//mesaj
        ///     "isError": false, // Hata durumunda true olur
        ///     "responseStatus": 0, //(0),Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///  }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        [HttpPut("updateProfile")]
        [Authorize]
        public async ValueTask<IResponseResult> UpdateUserName([FromForm] DTOUserProfile model)
        {
            return await _userManagerService.UpdateUserProfileAsync(model);
        }


        /// <summary>
        /// Kullanıcı şifre güncelleme . Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     PUT /api/changePassword
        ///     {
        ///         "UserId": "UserId Kullanıcı Id zorunlu alan!",
        ///         "OldPassword": "Mevcut şifre 123456 olabilir zorunlu alan!"
        ///         "NewPassword": "Yeni şifre  123456 olabilir zorunlı alan!",
        ///     }
        /// 
        /// Başarılı istek sonucu:
        /// 
        /// {
        ///     "message": "Şifre Başarıyla güncellendi",//Mesaj
        ///     "isError": false, // Hata durumunda true olur
        ///     "responseStatus": 0, //(0),Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///  }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        [HttpPut("changePassword")]
        [Authorize]
        public async ValueTask<IResponseResult> ChangePassword(DTOUserPassword model)
        {
            return await _userManagerService.UpdatePassword(model);
        }

        /// <summary>
        /// Kullanıcı çekme . Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     GET /api/user/id
        ///     {
        ///         "Id": "UserId Kullanıcı Id zorunlu alan!",
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        [HttpGet("user/{id}")]
        [Authorize]
        public async ValueTask<DTOUser> User(string id)
        {
            return await _userManagerService.GetUserAsync(id);
        }

        /// <summary>
        /// Kullanıcı arama . Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     GET /api/user/searchUsers
        ///     {
        ///         "UserName": "Kullanıcı ismi zorunlu alan!",
        ///     }
        /// </remarks>
        /// <param name="username"></param>
        [HttpGet("searchUsers")]
        [Authorize]
        public List<DTOUserSearch> GetSearchUsers(string username)
        {
            return _userManagerService.GetSearchUser(username);
        }
    }
}
