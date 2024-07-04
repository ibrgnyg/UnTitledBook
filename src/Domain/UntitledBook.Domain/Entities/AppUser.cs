using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UntitledBook.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        private const string defaultUser = "https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-image-182145777.jpg";

        public string ImagePath { get; set; } = defaultUser;
        public ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();
    }
}
