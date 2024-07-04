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
    public class FriendsProfile:Profile
    {
        public FriendsProfile()
        {
            CreateMap<DTOFriendsList, Friendship>();
            CreateMap<Friendship, DTOFriendsList>();

            CreateMap<DTOFriendsList, AppUser>();
            CreateMap<AppUser, DTOFriendsList>();
        }
    }
}
