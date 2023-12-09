using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.PatientDto
{
	public class AddBookingDto
	{
		public int DoctorId { get; set; }
		public int TimeId { get; set; }
		public int DayId { get; set; }
		public int DiscountId { get; set; }
	}
}
