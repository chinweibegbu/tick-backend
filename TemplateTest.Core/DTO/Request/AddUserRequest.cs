using TemplateTest.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace TemplateTest.Core.DTO.Request
{
    public class AddUserRequest
    {
        [Required]
        [MinLength(4)]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public UserRole Role { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string ParticipantId { get; set; }
    }
}
