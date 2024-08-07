using Tick.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Tick.Core.DTO.Request
{
    public class EditTaskRequest
    {
        [Required]
        public string Details { get; set; } = null! ;

        [Required]
        public bool? IsImportant { get; set; }
    }
}
