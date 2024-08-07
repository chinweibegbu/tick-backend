using Tick.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Tick.Core.DTO.Request
{
    public class AddTaskRequest
    {
        [Required]
        public string Details { get; set; }

        [Required]
        public bool IsImportant { get; set; }
    }
}
