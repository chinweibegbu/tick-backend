using TemplateTest.Domain.Enum;
using System;

namespace TemplateTest.Core.DTO.Response
{
    public class BasicAuthResponse
    {
        public string Id { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public BasicAuthStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
