using System.ComponentModel.DataAnnotations.Schema;
using Tick.Domain.Entities.Base;

namespace Tick.Domain.Entities
{
    public class Task : EntityBase
    {
        public Task()
        {
            SetNewId();
        }

        [Column("DETAILS")] 
        public string Details { get; set; }

        [Column("IS_IMPORTANT")]
        public bool IsImportant { get; set; }

        [Column("IS_COMPLETED")]
        public bool IsCompleted { get; set; }
        [Column("TICKER_ID")]

        public string TickerId {  get; set; }

        public Ticker Ticker { get; set; }
    }
}
