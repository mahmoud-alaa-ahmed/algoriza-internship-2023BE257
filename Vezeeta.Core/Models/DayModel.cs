
using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta.Core.Enums;

namespace Vezeeta.Core.Models
{
	public class DayModel: BaseEntity
	{
        public DayModel()
        {
            Times = new List<TimeModel>();
        }
        public Days Day { get; set; }
        public List<TimeModel> Times { get; set; }

        [ForeignKey(nameof(Appointment))]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = default!;

    }

}
