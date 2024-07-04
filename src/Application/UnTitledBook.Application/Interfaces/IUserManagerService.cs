using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledBook.Domain.Entities;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Application.Interfaces
{
    public interface IUserManagerService
    {
        ValueTask<IResponseResult> RegisterUserAsync(DTOUserRegister model);
        ValueTask<IResponseResult> LoginUserAsync(DTOUserLogin model);
        ValueTask<IResponseResult> UpdateUserProfileAsync(DTOUserProfile model);
        ValueTask<IResponseResult> UpdatePassword(DTOUserPassword model);
        ValueTask<DTOUser?> GetUserAsync(string id);
        List<DTOUserSearch> GetSearchUser(string username);
        ValueTask<IResponseResult> ExistUsers(string id);
    }
}
