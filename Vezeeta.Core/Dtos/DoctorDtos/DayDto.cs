using Vezeeta.Core.Enums;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos.DoctorDtos
{
	public class DayDto
	{
		public Days Day { get; set; }
		public List<TimeDto> Times { get; set; }

	}
}
