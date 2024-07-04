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
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<DTOUserSearch, AppUser>();
            CreateMap<AppUser, DTOUserSearch>();

            CreateMap<DTOUser, AppUser>();
            CreateMap<AppUser, DTOUser>();
 
        }
    }
}
