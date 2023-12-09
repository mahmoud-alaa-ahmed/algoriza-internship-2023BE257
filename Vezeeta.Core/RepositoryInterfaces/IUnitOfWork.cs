using Vezeeta.Core.Models;

namespace Vezeeta.Core.RepositoryInterfaces
{
	public interface IUnitOfWork:IDisposable
	{
		IBaseRepository<Doctor> Doctor { get; }
		IBaseRepository<Patient> Patient { get; }
		IBaseRepository<Specialization> Specialization { get; }
		IBaseRepository<Appointment> Appointment { get; }
		IBookingRepository Booking { get; }
		IBaseRepository<TimeModel> Time { get; }
		IBaseRepository<DayModel> Day { get; }
		IBaseRepository<Discount> Discount { get; }
		Task<int> SaveDataAsync();
	}
}
