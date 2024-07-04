using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledBook.Domain.Entities;
using UntitledBook.Domain.Enums;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;
using UnTitledBook.Infrastructure.Persistence.Context;

namespace UnTitledBook.Infrastructure.Persistence.Services
{
    public class BookService : IBookService
    {
        private readonly AppDBContext _dbContext;
        private readonly IResponseResult _responseResult;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IValidator<DTOBook> _validator;
        private readonly IUserManagerService _userManagerService;

        public BookService(AppDBContext dbContext, IResponseResult responseResult, IWebHostEnvironment environment, IMapper mapper, IValidator<DTOBook> validator, IUserManagerService userManagerService)
        {
            _dbContext = dbContext;
            _responseResult = responseResult;
            _environment = environment;
            _mapper = mapper;
            _validator = validator;
            _userManagerService = userManagerService;
        }

        public IList<DTOBookList> GetDTOBookLists(DTOFilter model)
        {
            List<DTOBookList> employeeDTOs = new List<DTOBookList>();
            try
            {
                var queryBook = _dbContext.Books.Where(x => x.UserId == model.UserId);

                if (!string.IsNullOrEmpty(model.SearchTerm))
                {
                    string lowerSearchTerm = model.SearchTerm.ToLower();
                    queryBook = queryBook.Where(x => x.Name.ToLower().Contains(lowerSearchTerm));
                }

                switch (model.FilterDate)
                {
                    case FilterDateType.NewAdded:
                        queryBook = queryBook.OrderByDescending(x => x.CreatedDate);
                        break;
                    case FilterDateType.OldAdded:
                        queryBook = queryBook.OrderBy(x => x.CreatedDate);
                        break;
                }

                List<Book> booksDB = queryBook.ToList();
                employeeDTOs = _mapper.Map<List<DTOBookList>>(booksDB);
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return employeeDTOs;
        }

        public IResponseResult AddBook(DTOBook model)
        {
            try
            {
                var validation = _validator.Validate(model);

                if (!validation.IsValid)
                {
                    var errorList = validation.Errors.Cast<object>().ToList();
                    return _responseResult.SetErrorValidationResponse(errorList);
                }

                var bookEntity = _mapper.Map<Book>(model);
                bookEntity.Id = Guid.NewGuid().ToString();

                if (!string.IsNullOrEmpty(bookEntity.UserId))
                {
                    var existUser = _userManagerService.ExistUsers(model.UserId).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (existUser.IsError)
                        return existUser;
                }

                if (model.File != null)
                {
                    bookEntity.ImagePath = UploadFilePath(bookEntity, model);
                }

                _dbContext.Books.Add(bookEntity);
                var result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    _responseResult.Fields = new Dictionary<string, object>()
                    {
                        {"BookId",bookEntity.Id}
                    };

                    return _responseResult.SetSuccessResponse("Kitap başarıyla eklendi");
                }
                return _responseResult.SetErrorResponse("Kitap eklenemedi!");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }

            return _responseResult;
        }

        public IResponseResult UpdateBook(DTOBook model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                    return _responseResult.SetErrorResponse("Id boş olamaz!");

                var book = _dbContext.Books.FirstOrDefault(b => b.Id == model.Id);

                if (book == null)
                {
                    return _responseResult.SetErrorResponse("Kitap bulunamadı!");
                }

                if (!string.IsNullOrEmpty(model.UserId))
                {
                    var existUser = _userManagerService.ExistUsers(model.UserId).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (existUser.IsError)
                        return existUser;
                }

                book.Name = model.Name;
                book.Author = model.Author;
                book.ISBN = model.ISBN;
                book.Description = model.Description;
                book.UserId = model.UserId;
                book.Room = model.Room;
                book.Section = model.Section;
                book.ShelfNumber = model.ShelfNumber;

                book.UpdateDate = DateTime.Now;
                if (model.File != null)
                {
                    book.ImagePath = UploadFilePath(book,model);
                }
                else
                {
                    book.ImagePath = "https://dynamicmediainstitute.org/wp-content/themes/dynamic-media-institute-theme/imagery/default-book.png";
                }

                _dbContext.Books.Update(book);
                var result = _dbContext.SaveChanges();

                if (result > 0)
                {
                    return _responseResult.SetSuccessResponse("Kitap başarıyla güncellendi");
                }

                return _responseResult.SetErrorResponse("Kitap güncellenemedi!");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        public IResponseResult DeleteBook(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return _responseResult.SetErrorResponse("Id boş olamaz!");

                var getBook = _dbContext.Books.FirstOrDefault(x => x.Id == id);
                if (getBook == null)
                    return _responseResult.SetErrorResponse("Kitap bulunamadı!");

                var result = _dbContext.Books.Remove(getBook);
                if (result.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                {
                    _dbContext.SaveChanges();
                    return _responseResult.SetSuccessResponse("Kitap silindi");
                }
                _responseResult.SetErrorResponse("Kitap silinemedi!");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        public DTOBook? GetDTOBook(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return null;

                var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);

                if (book == null)
                    return null;

                var mappBook = _mapper.Map<DTOBook>(book);
                return mappBook;
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return null;
        }

        private string UploadFilePath(Book book, DTOBook model)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "static", model.UserId);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, model.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            if (!string.IsNullOrEmpty(book.ImagePath) && System.IO.File.Exists(book.ImagePath))
            {
                System.IO.File.Delete(book.ImagePath);
            }

            var relativePath = $"/static/{model.UserId}/{model.File.FileName}";
            return book.ImagePath = relativePath;
        }

    }
}
