
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SimpleStore.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Column(TypeName ="decimal(10,1)")]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        
        public IEnumerable<OrderDetail>? OrderDetails { get; set; }
    }
}
