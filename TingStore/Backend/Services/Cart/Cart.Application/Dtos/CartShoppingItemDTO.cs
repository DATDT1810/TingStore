﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Application.Dtos
{
    public class CartShoppingItemDTO
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
    }
}
