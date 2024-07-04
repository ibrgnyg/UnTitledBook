using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UntitledBook.Domain.Configurations;
using UntitledBook.Domain.Entities;
using UntitledBook.Domain.Enums;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;

namespace UnTitledBook.Infrastructure.Persistence.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IResponseResult _responseResult;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IValidator<DTOUserRegister> _validatorRegister;
        private readonly IValidator<DTOUserLogin> _validatorLogin;
        private readonly IValidator<DTOUserProfile> _validatorProfile;
        private readonly JwtConfig _jwtConfig;
        private readonly IMapper _mapper;
        public UserManagerService(IResponseResult responseResult,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IValidator<DTOUserRegister> validatorRegister,
            IValidator<DTOUserLogin> validatorLogin, IOptions<JwtConfig> jwt,
            IValidator<DTOUserProfile> validatorProfile, IWebHostEnvironment environment, IMapper mapper)
        {
            _responseResult = responseResult;
            _userManager = userManager;
            _signInManager = signInManager;
            _validatorRegister = validatorRegister;
            _validatorLogin = validatorLogin;
            _jwtConfig = jwt.Value;
            _validatorProfile = validatorProfile;
            _environment = environment;
            _mapper = mapper;
        }

        public virtual async ValueTask<IResponseResult> RegisterUserAsync(DTOUserRegister model)
        {
            try
            {
                var validation = _validatorRegister.Validate(model);

                if (!validation.IsValid)
                {
                    var errorList = validation.Errors.Cast<object>().ToList();
                    return _responseResult.SetErrorValidationResponse(errorList);
                }

                var existEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existEmail != null)
                {
                    return _responseResult.SetErrorResponse($"Kayıtlı e-posta {model.Email}");
                }

                var user = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };

                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (!identityResult.Succeeded)
                {
                    if (identityResult.Errors.Any(x => x.Code == "DuplicateUserName"))
                    {
                        return _responseResult.SetErrorResponse("Kullanıcı adı mevcut!");
                    }

                    return _responseResult.SetErrorResponse("Hata üye olunamadı!");
                }
                return _responseResult.SetSuccessResponse("Üye olundu şimdi giriş yapabilirsin");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        public virtual async ValueTask<IResponseResult> LoginUserAsync(DTOUserLogin model)
        {
            try
            {
                var validation = _validatorLogin.Validate(model);

                if (!validation.IsValid)
                {
                    var errorList = validation.Errors.Cast<object>().ToList();
                    return _responseResult.SetErrorValidationResponse(errorList);
                }

                var appUser = await _userManager.FindByEmailAsync(model.Email);

                if (appUser == null)
                    return _responseResult.SetErrorResponse("Kullanıcı bulunamadı!");

                var identityResult = await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);
                if (!identityResult.Succeeded)
                {
                    return _responseResult.SetErrorResponse("E-posta veya şifre hatalı!");
                }

                var token = CreateJwtToken(appUser);

                _responseResult.Fields = new Dictionary<string, object>()
                {
                    {"UserId",appUser.Id},
                    {"Token",token}
                };

                return _responseResult.SetSuccessResponse("");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        private string CreateJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public virtual async ValueTask<DTOUser?> GetUserAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return null;

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return null;

                var mapperUser =_mapper.Map<DTOUser>(user);

                return mapperUser;
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return null;
        }

        public virtual async ValueTask<IResponseResult> UpdateUserProfileAsync(DTOUserProfile model)
        {
            try
            {
                var validation = _validatorProfile.Validate(model);

                if (!validation.IsValid)
                {
                    var errorList = validation.Errors.Cast<object>().ToList();
                    return _responseResult.SetErrorValidationResponse(errorList);
                }

                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user == null)
                    return _responseResult.SetErrorResponse("Kullanıcı bulunamadı!");

                if (model.File != null)
                {
                    var uploadPath = Path.Combine(_environment.WebRootPath, "static", model.UserId);

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, model.File.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.File.CopyTo(stream);
                    }

                    var relativePath = $"/static/{model.UserId}/{model.File.FileName}";
                    user.ImagePath = relativePath;
                }
                else if (string.IsNullOrEmpty(user.ImagePath))
                {
                    user.ImagePath = "https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-image-182145777.jpg";
                }

                user.UserName = model.UserName;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    return _responseResult.SetErrorResponse("Profil bilgisi güncellenemedi!");
                }
                return _responseResult.SetSuccessResponse("Profil bilgisi güncellendi");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        public virtual async ValueTask<IResponseResult> UpdatePassword(DTOUserPassword model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user == null)
                    return _responseResult.SetErrorResponse("Kullanıcı bulunamadı!");

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    if (result.Errors.Any(x => x.Code == "PasswordMismatch"))
                    {
                        return _responseResult.SetErrorResponse("Şireler eşleşmiyor!");
                    }
                    return _responseResult.SetSuccessResponse("Şifre güncellenemedi!");
                }
                return _responseResult.SetSuccessResponse("Şifre başarıyla güncellendi");

            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }

        public List<DTOUserSearch> GetSearchUser(string username)
        {
            var dTOUserSearches = new List<DTOUserSearch>();
            try
            {
                if (string.IsNullOrEmpty(username))
                    return null;

                var searchUsers = _userManager.Users.Where(x => x.UserName.ToLower().Contains(username.ToLower())).ToList();

                dTOUserSearches = _mapper.Map<List<DTOUserSearch>>(searchUsers);
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return dTOUserSearches;
        }

        public virtual async ValueTask<IResponseResult> ExistUsers(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return _responseResult.SetErrorResponse("Kullanıcı bulunamadı!");

                return _responseResult.SetSuccessResponse("Kullanıcı bulundu");
            }
            catch (Exception ex)
            {
                _responseResult.SetExceptionResponse(ex);
            }
            return _responseResult;
        }
    }
}
