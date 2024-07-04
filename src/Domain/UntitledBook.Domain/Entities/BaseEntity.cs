using System.ComponentModel.DataAnnotations.Schema;

namespace UntitledBook.Domain.Entities
{
    public class BaseEntity
    {
        [ForeignKey("Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
}
