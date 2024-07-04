using UntitledBook.Domain.Enums;

namespace UntitledBook.Domain.Entities
{
    public class Note : BaseEntity
    {
        /// <summary>
        /// Note
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Not tipi (0: Özel, 1: Sadece Arkadaşlar, 2: Herkese Açık)
        /// </summary>
        public NoteType NoteType { get; set; } = NoteType.Public;

        /// <summary>
        /// Kitap Id
        /// </summary>
        public string BookId { get; set; } = string.Empty;

        /// <summary>
        /// Arkadaş Id  (NoteType: Friends olduğunda kullanılır)
        /// </summary>
        public string? FriendUserId { get; set; }

        /// <summary>
        /// Kullanıcı ID'si
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// // Notu oluşturan kullanıcı
        /// </summary>
        public virtual AppUser? User { get; set; }
        /// <summary>
        /// Arkadaş
        /// </summary>
        public virtual AppUser Friend { get; set; }
        public virtual Book Book { get; set; }
    }
}
