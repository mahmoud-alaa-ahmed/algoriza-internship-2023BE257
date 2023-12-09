using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.DoctorDtos
{
	public class DeleteTimeRequestDto
	{
		public int AppointmentId { get; set; }
		public int DayId { get; set; }
		public int TimeId { get; set; }
	}
}
