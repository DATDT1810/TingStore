﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Category.Application.Queries
{
    public class DeleteCategoryByIdQuery : IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteCategoryByIdQuery(string id)
        {
            Id = id;
        }
    }
}
