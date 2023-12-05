
using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta.Core.Models
{
	public class TimeModel: BaseEntity
	{
        public TimeSpan Time { get; set; }

        [ForeignKey(nameof(Day))]
        public int DayId { get; set; }
        public DayModel Day { get; set; } = default!;
    }

}
