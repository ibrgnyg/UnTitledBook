using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Security.Principal;
using System.Text;
using UntitledBook.Domain.Configurations;
using UntitledBook.Domain.Entities;
using UnTitledBook.Application.DTOs;
using UnTitledBook.Application.Interfaces;
using UnTitledBook.Application.Response;
using UnTitledBook.Infrastructure.Persistence.Context;
using UnTitledBook.Infrastructure.Persistence.Services;
using UnTitledBook.Infrastructure.Persistence.Validators;

namespace UnTitledBook.Infrastructure
{
    public static class Builder
    {
        public static void ConfigureServices(IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false; 
                options.Password.RequireNonAlphanumeric = false; 
                options.Password.RequireUppercase = false; 
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1; 
                options.User.RequireUniqueEmail = true; 
                options.User.AllowedUserNameCharacters = "abcçdefghiıjklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789_-";
                options.Lockout.AllowedForNewUsers = true; 
            }).AddEntityFrameworkStores<AppDBContext>()
              .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = configuration["JwtConfig:Issuer"],
                       ValidAudience = configuration["JwtConfig:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Key"]))
                   };
               });


            var configurationSection = configuration.GetSection(nameof(JwtConfig));
            services.Configure<JwtConfig>(configurationSection);

            services.AddAutoMapper(typeof(Builder).Assembly);

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("mc", LogEventLevel.Information)
            .CreateLogger();

            services.AddScoped<IValidator<DTOUserLogin>, UserLoginValidator>();
            services.AddScoped<IValidator<DTOUserRegister>, UserRegisterValidator>();
            services.AddScoped<IValidator<DTOBook>, BookValidator>();
            services.AddScoped<IValidator<DTONote>, NoteValidator>();
            services.AddScoped<IValidator<DTOUserProfile>, UserProfileValidator>();

            //Services
            services.AddSingleton<IResponseResult, ResponseResult>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
        }

    }
}
