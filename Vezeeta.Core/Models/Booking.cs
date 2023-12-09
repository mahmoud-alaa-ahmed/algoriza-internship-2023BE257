using System.ComponentModel.DataAnnotations.Schema;
using Vezeeta.Core.Enums;

namespace Vezeeta.Core.Models
{
	public class Booking:BaseEntity
	{
		public BookingStatus Status { get; set; }

        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = default!;

		[ForeignKey(nameof(Time))]
		public int TimeId { get; set; }
		public TimeModel Time { get; set; } = default!;

		[ForeignKey(nameof(Day))]
		public int DayId { get; set; }
		public DayModel Day { get; set; } = default!;

		[ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

		[ForeignKey(nameof(Discount))]
        public int DiscountId { get; set; }
        public Discount Discount { get; set; } = default!;
		public double Price { get; set; }
		public double FinalPrice { get; set; }

	}
}
