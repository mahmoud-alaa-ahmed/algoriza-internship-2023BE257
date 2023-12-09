using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.DoctorDtos
{
	public class DoctorBookingResponseDto
	{
        public string PatientName { get; set; }
        public string PatientImage { get; set; }
        public string PatientGender { get; set; }
        public string PatientPhone { get; set; }
        public string PatientEmail { get; set; }
        public int PatientAge { get; set; }
    }
}
