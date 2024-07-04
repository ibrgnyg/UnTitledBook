using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Application.Interfaces
{
    public interface IFriendshipService
    {
        List<DTOFriendsList> GetFriendsLists(string userId);
        IResponseResult AddFriends(DTOFriend model);
        IResponseResult DeleteFriends(string id);
        IResponseResult ExistFriend(string userId, string friendId);
    }
}
