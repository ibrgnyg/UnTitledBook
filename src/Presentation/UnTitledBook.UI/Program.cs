using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UnTitledBook.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();

var info = new OpenApiInfo()
{
    Title = "Kitaplýk Arþiv API  Dökümaný",
    Version = "v1",
    Description = "Bu projede kullanýcýlar üye olma ve giriþ yapma iþlemlerini gerçekleþtirebilir. Giriþ yapan kullanýcýlar kitap ekleyebilir ve diðer kullanýcýlarý arkadaþ olarak ekleyebilirler. Ayrýca, kullanýcýlar kitaplarla ilgili notlar ekleyebilir ve bu notlarý herkese açýk veya sadece arkadaþlarýna özel olarak paylaþabilirler."
};

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", info);

    var xmlFile = $"UnTitledBook.Api.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Login istegi eger e-posta ve þifre dogru iste token döner",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

Builder.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", info.Title);
});
//}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/user/profile");
        return Task.CompletedTask;
    });
});

app.Run();
