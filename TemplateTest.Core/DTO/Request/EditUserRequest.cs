using TemplateTest.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace TemplateTest.Core.DTO.Request
{
    public class EditUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        public UserRole? Role { get; set; }

        [DataType(DataType.Text)]
        public string ParticipantId { get; set; }
    }
}
