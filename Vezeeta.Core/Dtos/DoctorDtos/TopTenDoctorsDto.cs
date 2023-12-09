using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.DoctorDtos
{
	public class TopTenDoctorsDto
	{
        public string FullName { get; set; }=string.Empty;
		public string Image { get; set; } = string.Empty;
		public string Specialize { get; set; } = string.Empty;
		public int NumOfRequests { get; set; }
	}
}
