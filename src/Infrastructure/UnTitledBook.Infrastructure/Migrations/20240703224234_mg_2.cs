using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnTitledBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f15f5116-bf9b-4126-b3f3-2c4363abaaea");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BookId", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "ImagePath", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "fc9773e5-a79b-41d0-b0f4-06ebdab5ff1d", 0, null, "1", "AppUser", "test@book.com", true, "https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-image-182145777.jpg", true, null, "TEST@BOOK.COM", "TESTBOOK", "AQAAAAEAACcQAAAAEDvyQ5fzBhHIGG+XvlWnHOPQFj8vzwsWagAXr+MVJnsKsfr5OKE4nInSitz6gyg3cA==", null, false, "X4J75DO73ZV6MVW7DRVYPDOIUHHZQ7PM", false, "testBook" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fc9773e5-a79b-41d0-b0f4-06ebdab5ff1d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BookId", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "ImagePath", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f15f5116-bf9b-4126-b3f3-2c4363abaaea", 0, null, "1", "AppUser", "test@book.com", true, "https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-image-182145777.jpg", true, null, "TEST@BOOK.COM", "TESTBOOK", "AQAAAAEAACcQAAAAEDvyQ5fzBhHIGG+XvlWnHOPQFj8vzwsWagAXr+MVJnsKsfr5OKE4nInSitz6gyg3cA==", null, false, "X4J75DO73ZV6MVW7DRVYPDOIUHHZQ7PM", false, "testBook" });
        }
    }
}
