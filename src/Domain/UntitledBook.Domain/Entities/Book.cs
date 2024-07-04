namespace UntitledBook.Domain.Entities
{
    public class Book : BaseEntity
    {
        private const string DefaultImage = "https://dynamicmediainstitute.org/wp-content/themes/dynamic-media-institute-theme/imagery/default-book.png";

        /// <summary>
        /// Kitabın ismi.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kitabın açıklaması.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcının ID'si.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Kitabın yazar ismi.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Kitabın ISBN numarası.
        /// </summary>
        public string ISBN { get; set; } = string.Empty;

        /// <summary>
        /// Kitap kapağı görüntü dosyasının yolu.
        /// </summary>
        public string ImagePath { get; set; } = DefaultImage;

        /// <summary>
        /// Kitabın bulunduğu oda.
        /// </summary>
        public string Room { get; set; } = string.Empty;

        /// <summary>
        /// Kitabın bulunduğu bölüm.
        /// </summary>
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// Kitabın bulunduğu raf numarası.
        /// </summary>
        public string ShelfNumber { get; set; } = string.Empty;

        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
        public virtual ICollection<AppUser> SharedWith { get; set; } = new List<AppUser>();
    }
}
