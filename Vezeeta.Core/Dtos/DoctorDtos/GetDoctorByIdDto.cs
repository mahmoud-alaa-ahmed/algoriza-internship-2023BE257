using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.DoctorDtos
{
	public class GetDoctorByIdDto: GetAllDoctorResponseDto
	{
        public DateTime DateOfBrith { get; set; }
    }
}
