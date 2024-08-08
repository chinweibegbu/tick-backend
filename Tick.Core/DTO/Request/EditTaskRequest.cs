using Tick.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Tick.Core.DTO.Request
{
    public class EditTaskRequest
    {
        public string Details { get; set; } = null! ;

        public bool? IsImportant { get; set; }
    }
}
