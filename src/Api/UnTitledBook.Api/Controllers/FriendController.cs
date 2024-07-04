using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;
using UnTitledBook.Infrastructure.Persistence.Services;

namespace UnTitledBook.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;
        public FriendController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        /// <summary>
        /// Arkadaş listeleme. Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek query parameterleri anlamları:
        /// 
        ///     GET /api/friendsLists
        ///     {        
        ///       "UserId": "Unique User Id zorunlu alan!",
        ///     }
        /// </remarks>
        /// <param name="userId"></param>   
        [HttpGet("friendsLists/{userId}")]
        public List<DTOFriendsList> GetFriendsLists(string userId)
        {
            return _friendshipService.GetFriendsLists(userId);
        }

        /// <summary>
        /// Arkadaş ekleme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/addFriend
        ///     {        
        ///         "UserId": "Unique User Id zorunlu alan!",
        ///       "FriendsId": "Arakdaş ekleyecek olan UserId zorunlu alan!",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Arkadaş başaryıla eklendi",//Mesaj
        ///         "isError": false, // Hata durumunda true olur
        ///         "responseStatus": 0, //(0)Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="model"></param>   
        [HttpPost("addFriend")]
        public IResponseResult AddFriend(DTOFriend model)
        {
            return _friendshipService.AddFriends(model);
        }

        /// <summary>
        /// Arkadaş listesinde arkadaş silme.  Bu istegi test edebilmek için authenticate zorunludur.
        /// </summary>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     DELETE /api/deleteFriends
        ///     {        
        ///       "Id": "Id zorunlu",
        ///     }
        ///     Başarılı istek sonucu:
        ///     {
        ///         "message": "Arkadaş başaryıla silindi",//mesaj
        ///         "isError": false,// Hata durumunda true olur
        ///         "responseStatus": 0,//(0)Success, (1)Error, (2)Exception,(3)EmptyParameter
        ///     }
        /// </remarks>
        /// <param name="id"></param>  
        [HttpDelete("deleteFriends/{id}")]
        public IResponseResult DeleteFriends(string id)
        {
            return _friendshipService.DeleteFriends(id);
        }
    }
}
