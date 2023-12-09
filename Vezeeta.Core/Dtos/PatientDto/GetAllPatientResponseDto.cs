using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.PatientDto
{
	public class GetAllPatientResponseDto
	{
		public string FullName { get; set; } = string.Empty;
		public string? Image { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public DateTime DateOfBrith { get; set; }
	}
}
