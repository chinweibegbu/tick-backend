using Tick.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Tick.Domain.QueryParameters
{
    public class TaskQueryParameters : UrlQueryParameters
    {
        public string Query { get; set; }
    }
}
