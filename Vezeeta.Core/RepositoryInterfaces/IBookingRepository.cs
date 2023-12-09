using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.RepositoryInterfaces
{
	public interface IBookingRepository: IBaseRepository<Booking>
	{
		Task<IEnumerable<Booking>> GetAllBookingAsync(int id);
		Task<IEnumerable<TopFiveSpecializationsDto>> TopFiveSpecializationsAsync();
		Task<IEnumerable<TopTenDoctorsDto>> TopTenDoctorsAsync();
	}
}
