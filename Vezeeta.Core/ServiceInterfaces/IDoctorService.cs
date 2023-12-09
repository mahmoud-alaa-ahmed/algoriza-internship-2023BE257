using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.ServiceInterfaces
{
	public interface IDoctorService
	{
		Task<AuthModel> LoginDoctorAsync(LoginDTO loginDTO);
		Task<IEnumerable<DoctorBookingResponseDto>> GetAllHisBoohingAsync(int doctorId, string searchBy = null, int pageSize = 10, int pageNumber = 1);
		Task<ConfirmCheckUpDto> ConfirmCheckUpAsync(int id);
		Task<Appointment> AddAppointmentAsync(AppointmentDto appointmentDto);
		Task<UpdateAppointmentResponseDto> UpdateAppointmentAsync(UpdateAppointmentTimeRequest updateAppointment);
		Task<DeleteAppointmentResponseDto> DeleteAppointmentAsync(DeleteTimeRequestDto deleteAppointment);
	}
}
