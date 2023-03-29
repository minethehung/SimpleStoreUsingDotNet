using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleStore.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DreatedDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(10,1)")]
        public decimal Total { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
