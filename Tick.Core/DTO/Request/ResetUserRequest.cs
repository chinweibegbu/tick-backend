using System.ComponentModel.DataAnnotations;

namespace Tick.Core.DTO.Request
{
    public class ResetUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
    }
}
