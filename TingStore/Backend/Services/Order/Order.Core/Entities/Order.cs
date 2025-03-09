using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.Entities
{
    public class Order
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalAmount { get; set; }
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Waiting";
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
