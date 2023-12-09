using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.PatientDto
{
	public class CancelBookingResponseDto
	{
        public bool IsCanceled { get; set; }
        public string Message { get; set; }
    }
}
