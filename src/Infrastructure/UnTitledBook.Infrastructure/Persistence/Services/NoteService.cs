using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UntitledBook.Domain.Entities;
using UntitledBook.Domain.Enums;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;
using UnTitledBook.Infrastructure.Persistence.Context;

namespace UnTitledBook.Infrastructure.Persistence.Services
{
    public class NoteService : INoteService
    {
        private AppDBContext _dbContext;
        private readonly IResponseResult _responseResult;
        private readonly IMapper _mapper;
        private readonly IValidator<DTONote> _validator;
        private readonly IFriendshipService _friendshipService;

        public NoteService(AppDBContext dbContext,
            IResponseResult responseResult,
            IMapper mapper,
            IValidator<DTONote> validator,
            IFriendshipService friendshipService)
        {
            _dbContext = dbContext;
            _responseResult = responseResult;
            _mapper = mapper;
            _validator = validator;
            _friendshipService = friendshipService;
        }

        public List<DTONoteList> SharedNotes(string userId)
        {
            List<DTONoteList> noteDTOs = new List<DTONoteList>();
            try
            {
                var notes = _dbContext.Notes.Where(x => x.FriendUserId == userId && x.NoteType == NoteType.FriendsOnly || x.NoteType == NoteType.Public).Include(x => x.Book).ToList();
                noteDTOs = _mapper.Map<List<DTONoteList>>(notes);
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return noteDTOs;
        }

        public List<DTONoteList> GetNotesBook(string bookId)
        {
            List<DTONoteList> noteDTOs = new List<DTONoteList>();
            try
            {
                var notes = _dbContext.Notes
                .Where(x => x.BookId == bookId)
                .Include(x => x.Friend)
                .ToList();
                noteDTOs = _mapper.Map<List<DTONoteList>>(notes);
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return noteDTOs;
        }

        public IResponseResult AddNote(DTONote model)
        {
            try
            {
                var validation = _validator.Validate(model);

                if (!validation.IsValid)
                {
                    var errorList = validation.Errors.Cast<object>().ToList();
                    return _responseResult.SetErrorValidationResponse(errorList);
                }


                void AddNoteEntity(Note noteEntity)
                {
                    _dbContext.Notes.Add(noteEntity);
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        _responseResult.SetSuccessResponse("Not başarıyla eklendi");
                    }
                    else
                    {
                        _responseResult.SetErrorResponse("Not eklenemedi!");
                    }
                }

                var noteEntities = new List<Note>();

                if (model.FriendIds != null && model.FriendIds.Count > 0)
                {
                    foreach (var friendId in model.FriendIds)
                    {
                        var noteEntity = _mapper.Map<Note>(model);

                        if (!string.IsNullOrEmpty(model.UserId) && string.IsNullOrEmpty(friendId))
                        {
                            var existFriendList = _friendshipService.ExistFriend(model.UserId, friendId);
                            if (existFriendList.IsError)
                                return existFriendList;
                        }

                        if (noteEntity.NoteType == NoteType.Public || noteEntity.NoteType == NoteType.Private)
                        {
                            noteEntity.FriendUserId = null;
                        }
                        else
                        {
                            noteEntity.FriendUserId = friendId;
                        }
                        noteEntities.Add(noteEntity);
                    }
                }
                else
                {
                    var noteEntity = _mapper.Map<Note>(model);
                    if (noteEntity.NoteType == NoteType.Private || noteEntity.NoteType == NoteType.Public)
                    {
                        noteEntity.FriendUserId = null;
                    }
                    noteEntities.Add(noteEntity);
                }

                foreach (var noteEntity in noteEntities)
                {
                    AddNoteEntity(noteEntity);
                }
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }


        public IResponseResult UpdateNote(string id, DTONote model)
        {
            try
            {
                var validation = _validator.Validate(model);

                if (!validation.IsValid)
                {
                    var errorList = validation.Errors.Cast<object>().ToList();
                    return _responseResult.SetErrorValidationResponse(errorList);
                }

                var existingNote = _dbContext.Notes.FirstOrDefault(x => x.Id == id);

                if (existingNote == null)
                    return _responseResult.SetErrorResponse("Güncellenecek not bulunamadı!");

                _mapper.Map(model, existingNote);

                if (existingNote.NoteType == NoteType.Public || existingNote.NoteType == NoteType.Private)
                {
                    existingNote.FriendUserId = null;
                }
                else if (existingNote.NoteType == NoteType.FriendsOnly && model.FriendIds != null && model.FriendIds.Any())
                {
                    existingNote.FriendUserId = model.FriendIds.FirstOrDefault();

                    if (!string.IsNullOrEmpty(model.UserId) && string.IsNullOrEmpty(existingNote.FriendUserId))
                    {
                        var existFriendList = _friendshipService.ExistFriend(model.UserId, existingNote.FriendUserId);
                        if (existFriendList.IsError)
                            return existFriendList;
                    }
                }

                _dbContext.Notes.Update(existingNote);

                var result = _dbContext.SaveChanges();

                if (result > 0)
                {
                    return _responseResult.SetSuccessResponse("Not başarıyla güncellendi.");
                }
                else
                {
                    return _responseResult.SetErrorResponse("Not güncellenemedi.");
                }
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
                return _responseResult;
            }
        }

        public IResponseResult DeleteNote(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return _responseResult.SetErrorResponse("Id boş olamaz!");

                var getNote = _dbContext.Notes.FirstOrDefault(x => x.Id == id);
                if (getNote == null)
                    return _responseResult.SetErrorResponse("Not bulunamadı!");

                var result = _dbContext.Notes.Remove(getNote);
                if (result.State == EntityState.Deleted)
                {
                    _dbContext.SaveChanges();
                    return _responseResult.SetSuccessResponse("Not silindi");
                }
                _responseResult.SetErrorResponse("Not silinemedi!");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

    }
}
