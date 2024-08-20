using System.ComponentModel.DataAnnotations;

namespace Tick.Core.DTO.Request
{
    public class ResetUserRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
