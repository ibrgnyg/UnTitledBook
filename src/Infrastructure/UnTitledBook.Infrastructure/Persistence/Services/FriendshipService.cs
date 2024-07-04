using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledBook.Domain.Entities;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;
using UnTitledBook.Infrastructure.Persistence.Context;

namespace UnTitledBook.Infrastructure.Persistence.Services
{
    public class FriendshipService : IFriendshipService
    {
        private AppDBContext _dbContext;
        private readonly IResponseResult _responseResult;
        private readonly IMapper _mapper;

        public FriendshipService(AppDBContext dbContext, IResponseResult responseResult, IMapper mapper)
        {
            _dbContext = dbContext;
            _responseResult = responseResult;
            _mapper = mapper;
        }

        public List<DTOFriendsList> GetFriendsLists(string userId)
        {
            try
            {
                var friends = _dbContext.Friendships
                                        .Where(x => x.UserId == userId)
                                        .Include(x => x.Friend)
                                        .ToList();

                return _mapper.Map<List<DTOFriendsList>>(friends);
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
                return new List<DTOFriendsList>();
            }
        }

        public IResponseResult AddFriends(DTOFriend model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.FriendsId))
                    return _responseResult.SetErrorResponse("UserId ve FriendsId boş olamaz!");

                if (_dbContext.Friendships.Any(x => x.UserId == model.UserId && x.FriendUserId == model.FriendsId))
                {
                    return _responseResult.SetErrorResponse("Arkadaşlar listede ekli");
                }

                _dbContext.Friendships.Add(new UntitledBook.Domain.Entities.Friendship()
                {
                    UserId = model.UserId,
                    FriendUserId = model.FriendsId
                });

                var result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    return _responseResult.SetSuccessResponse("Arkadaş eklendi");
                }
                return _responseResult.SetErrorResponse("Arkadaş eklenemedi!");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        public IResponseResult DeleteFriends(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return _responseResult.SetErrorResponse("Id boş olamaz!");

                var getFriendship = _dbContext.Friendships.FirstOrDefault(x => x.Id == id);
                if (getFriendship == null)
                    return _responseResult.SetErrorResponse("Arkadaş bulunamadı!");

                var result = _dbContext.Friendships.Remove(getFriendship);
                if (result.State == EntityState.Deleted)
                {
                    _dbContext.SaveChanges();
                    return _responseResult.SetSuccessResponse("Arkadaş silindi");
                }
                _responseResult.SetErrorResponse("Arkadaş silinemedi!");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        public IResponseResult ExistFriend(string userId, string friendId)
        {
            try
            {
                var friendship = _dbContext.Friendships.Where(x=>x.UserId == userId && x.FriendUserId ==friendId).FirstOrDefault();

                if (friendship == null)
                    return _responseResult.SetErrorResponse("Arkadaş bulunamadı!");

                return _responseResult.SetSuccessResponse("Arkadaş bulundu");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }
    }
}
