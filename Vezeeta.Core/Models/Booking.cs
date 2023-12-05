using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta.Core.Enums;

namespace Vezeeta.Core.Models
{
	public class Booking:BaseEntity
	{
		public BookingStatus Status { get; set; }

        [ForeignKey(nameof(Appointment))]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = default!;

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

		[ForeignKey(nameof(Discount))]
        public int DiscountId { get; set; }
        public Discount Discount { get; set; } = default!;

		public double FinalPrice { get; set; }

	}
}
