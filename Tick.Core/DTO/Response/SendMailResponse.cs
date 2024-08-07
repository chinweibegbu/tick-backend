namespace Tick.Core.DTO.Response
{
    public class SendMailResponse
    {
        public bool succeeded { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
