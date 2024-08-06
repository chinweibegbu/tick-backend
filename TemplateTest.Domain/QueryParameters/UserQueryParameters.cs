using TemplateTest.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace TemplateTest.Domain.QueryParameters
{
    public class UserQueryParameters : UrlQueryParameters
    {
        [DataType(DataType.Text)]
        public string Query { get; set; }
        [DataType(DataType.Text)]
        public UserRole? Role { get; set; }
        [DataType(DataType.Text)]
        public UserStatus? Status { get; set; }
    }
}
