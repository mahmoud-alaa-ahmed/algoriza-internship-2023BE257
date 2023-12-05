
using System.ComponentModel.DataAnnotations.Schema;

namespace Vezeeta.Core.Models
{
	public class Appointment: BaseEntity
	{
        public Appointment()
        {
            Days = new List<DayModel>();
        }
        public double Price { get; set; }
        public ICollection<DayModel> Days { get; set; }
        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = default!;
    }

}
