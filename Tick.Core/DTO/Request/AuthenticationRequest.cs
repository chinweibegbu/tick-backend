using System.ComponentModel.DataAnnotations;

namespace Tick.Core.DTO.Request
{
    public class AuthenticationRequest
    {
        [DataType(DataType.Text)]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Password { get; set; }
    }
}
