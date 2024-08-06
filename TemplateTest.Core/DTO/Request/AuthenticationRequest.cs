using System.ComponentModel.DataAnnotations;

namespace TemplateTest.Core.DTO.Request
{
    public class AuthenticationRequest
    {
        [DataType(DataType.Text)]
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Password { get; set; }
    }
}
