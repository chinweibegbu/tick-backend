using System;

namespace Tick.Domain.Common
{
    public class DynamicEmailTemplateData : Object
    {
        public object token { get; set; }
        public string email { get; set; }
        public DynamicEmailTemplateData(string t, string e)
        {
            token = t;
            email = e;
        }
    }
}
