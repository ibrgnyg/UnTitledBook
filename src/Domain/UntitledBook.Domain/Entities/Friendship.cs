namespace UntitledBook.Domain.Entities
{
    public class Friendship : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string FriendUserId { get; set; } = string.Empty;
        public AppUser Friend { get; set; }
    }
}
