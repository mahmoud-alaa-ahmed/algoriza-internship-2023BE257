using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos.PatientDto
{
	public class GettPatientAllHisBokingDto
	{
        public string DoctorImage { get; set; }
        public string DoctorName { get; set; }
        public string Specialize { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public double Price { get; set; }
        public string DiscountCode { get; set; }
        public double FinalPrice { get; set; }
        public string Status { get; set; }
    }
}
