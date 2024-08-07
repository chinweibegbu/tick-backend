using Tick.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Tick.Core.DTO.Response
{
    public class TaskResponse
    {
        public string Id { get; set; }
        public string TickerId { get; set; }
        public string Details { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }
    }
}
