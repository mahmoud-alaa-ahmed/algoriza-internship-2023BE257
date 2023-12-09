using Vezeeta.Core.Models;
using Vezeeta.Core.RepositoryInterfaces;
using Vezeeta.Infrastructure.Data;

namespace Vezeeta.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public IBaseRepository<Doctor> Doctor { get; private set; }

		public IBaseRepository<Patient> Patient { get; private set; }

		public IBaseRepository<Specialization> Specialization { get; private set; }

		public IBaseRepository<Appointment> Appointment { get; private set; }

		public IBookingRepository Booking { get; private set; }

		public IBaseRepository<TimeModel> Time { get; private set; }

		public IBaseRepository<DayModel> Day { get; private set; }

		public IBaseRepository<Discount> Discount { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
			_context = context;
			Doctor = new BaseRepository<Doctor>(_context);
			Patient = new BaseRepository<Patient>(_context);
			Specialization = new BaseRepository<Specialization>(_context);
			Appointment = new BaseRepository<Appointment>(_context);
			Booking = new BookingRepository(_context);
			Time = new BaseRepository<TimeModel>(_context);
			Day = new BaseRepository<DayModel>(_context);
			Discount = new BaseRepository<Discount>(_context);
		}

        public void Dispose()
		{
			_context.Dispose();
		}

		public async Task<int> SaveDataAsync()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
