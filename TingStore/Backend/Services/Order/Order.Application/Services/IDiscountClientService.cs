﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public interface IDiscountClientService
    {
        public Task<decimal> GetValue();
    }
}
