﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.Application.Dtos;
using MediatR;

namespace Cart.Application.Queries
{
   public class GetCartByIdQuery : IRequest<CartShoppingDTO>
    {
       public int Id { get; set; }
        public GetCartByIdQuery(int id)
        {
            Id = id;
        }
    }
}
