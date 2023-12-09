using Microsoft.EntityFrameworkCore;
using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.RepositoryInterfaces;
using Vezeeta.Infrastructure.Data;

namespace Vezeeta.Infrastructure.Repositories
{
	public class BookingRepository : BaseRepository<Booking>, IBookingRepository
	{
		private readonly ApplicationDbContext _context;

		public BookingRepository(ApplicationDbContext context):base(context)
        {
			this._context = context;
		}
        public async Task<IEnumerable<Booking>> GetAllBookingAsync(int id)
		{
			var bookings = _context.Bookings
				.Where(b => b.PatientId == id)
				.Include(b => b.Doctor)
				.ThenInclude(d => d.Specialization)
				.Include(b => b.Doctor)
				.ThenInclude(d => d.User)
				.Include(b => b.Day)
				.Include(b => b.Time)
				.Include(b => b.Discount);
		  
			return await bookings.ToListAsync();
		}

		public async Task<IEnumerable<TopFiveSpecializationsDto>> TopFiveSpecializationsAsync()
		{
			var topDoctors = await _context.Doctors.Include(d => d.Specialization)
			.Select(d => new
			{
				Doctor = d,
				NumberOfRequests = _context.Bookings.Count(b => b.DoctorId == d.Id)
			})
			.OrderByDescending(x => x.NumberOfRequests)
			.Take(5)
			.Select(x => new TopFiveSpecializationsDto
			{
				FullName = x.Doctor.Specialization.Name,
				NumOfRequests = x.NumberOfRequests
			})
			.ToListAsync();
			return topDoctors;
		}

		public async Task<IEnumerable<TopTenDoctorsDto>> TopTenDoctorsAsync()
		{

			var topDoctors = await _context.Doctors
				.Include(d => d.Specialization)
				.Include(d => d.User)
			.Select(d => new
			{
				Doctor = d,
				NumberOfRequests = _context.Bookings.Count(b => b.DoctorId == d.Id)
			})
			.OrderByDescending(x => x.NumberOfRequests)
			.Take(5)
			.Select(x => new TopTenDoctorsDto
			{
				Specialize = x.Doctor.Specialization.Name,
				FullName = $"{x.Doctor.User.FirstName} {x.Doctor.User.LastName}",
				Image = x.Doctor.User.Image,
				NumOfRequests = x.NumberOfRequests
			})
			.ToListAsync();

			return topDoctors;
		}
	}
}
