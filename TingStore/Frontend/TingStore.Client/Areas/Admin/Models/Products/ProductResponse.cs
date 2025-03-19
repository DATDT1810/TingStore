using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TingStore.Client.Areas.Admin.Models.Products
{
    public class ProductResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }
        public DateTime CreateAt { get; set; }
        public string CategoryId { get; set; }
        public bool IsActive { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}
