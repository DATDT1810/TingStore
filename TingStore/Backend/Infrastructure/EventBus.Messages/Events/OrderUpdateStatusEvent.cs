﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
   public class OrderUpdateStatusEvent
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }

    }
}
