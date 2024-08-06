namespace TemplateTest.Core.DTO.Request
{
    public class SendMailRequest
    {
        public string from { get; set; }
        public string displayName { get; set; } = "TemplateTest Portal Admin";
        public string to { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string mailMessage { get; set; }
        public string subject { get; set; }
    }
}
