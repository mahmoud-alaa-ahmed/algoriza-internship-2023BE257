using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.PatientDto;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.ServiceInterfaces
{
	public interface IPatientService
	{
		Task<AuthModel> RegisterAsync(RegisterDTO registerDTO);
		Task<AuthModel> LoginAsync(LoginDTO loginDTO);
		Task<IEnumerable<GettPatientAllHisBokingDto>> GetAllHisBooking(string userId);
		Task<CancelBookingResponseDto> CancelBookingAsync(string userId, int bookingId);
		Task<BookingResponseDto> AddBooking(AddBookingDto dto, string userId);
	}
}
