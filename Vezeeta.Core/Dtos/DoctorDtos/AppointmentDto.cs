namespace Vezeeta.Core.Dtos.DoctorDtos
{
	public class AppointmentDto
	{
		public double Price { get; set; }
		public int DoctorId { get; set; }
        public List<DayDto> Days { get; set; }
    }
}
