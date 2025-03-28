﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Payment.Application.Dtos;

namespace Payment.Application.Commands
{
    public class AddTransactionCommand : IRequest<PaymentTransactionDTO>
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; } 
        public string? TransactionId { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;      
    }
}
