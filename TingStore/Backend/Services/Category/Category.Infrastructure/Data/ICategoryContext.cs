﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Category.Infrastructure.Data
{
    public interface ICategoryContext
    {
        IMongoCollection<Category.Core.Entities.Category> Categories { get; set; }
    }
}
