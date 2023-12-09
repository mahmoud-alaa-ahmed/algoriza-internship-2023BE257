using Vezeeta.Core.Enums;

namespace Vezeeta.Core.Dtos.PatientDto
{
	public class RequestDto
	{
        public string DoctorImage { get; set; }
        public string DoctorNmae { get; set; }
        public string Specialize { get; set; }
		public string Day { get; set; }
        public string Time { get; set; }
        public string discoundCode { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }
        public BookingStatus Status { get; set;}
    }
}
