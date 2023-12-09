using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Enums;
using Vezeeta.Core.Models;
using Vezeeta.Core.RepositoryInterfaces;
using Vezeeta.Core.ServiceInterfaces;
using Vezeeta.Infrastructure.Data;

namespace Vezeeta.Services
{
	public class DoctorService : IDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ApplicationDbContext _context;
		private readonly IAuth _auth;

		public DoctorService(IUnitOfWork unitOfWork, ApplicationDbContext context, IAuth auth)
		{
			this._unitOfWork = unitOfWork;
			this._context = context;
			_auth = auth;
		}
		public async Task<Appointment> AddAppointmentAsync(AppointmentDto appointmentDto)
		{
			using(var tran = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					Appointment app = new() {Price = appointmentDto.Price,DoctorId = appointmentDto.DoctorId };
					await _context.AddAsync(app);
					await _context.SaveChangesAsync();
					foreach (var daydto in appointmentDto.Days)
					{
						DayModel day = new DayModel
						{
							AppointmentId = app.Id,
							Day = daydto.Day,
						};
						await _context.AddAsync(day);
						await _context.SaveChangesAsync();
						foreach (var timedto in daydto.Times)
						{
							TimeModel time = new TimeModel
							{
								DayId = day.Id,
								Time = TimeSpan.Parse(timedto.Time),
							};
							await _context.AddRangeAsync(time);
							await _context.SaveChangesAsync();
						}
					}
					await tran.CommitAsync();
					return app;
				}
				catch (Exception)
				{
					await tran.RollbackAsync();
					return null;
				}
			}
		}

		public async Task<ConfirmCheckUpDto> ConfirmCheckUpAsync(int id)
		{
			var booking = await _context.Bookings
				.Include(b => b.Doctor)
				.Include(b => b.Patient)
				.FirstOrDefaultAsync(b => b.Id == id);
			if (booking is null)
				return new ConfirmCheckUpDto
				{
					IsConfirmed = false,
					Message = "Booking not found."
				};
			if (booking.Status == BookingStatus.Confirmed)
				return new ConfirmCheckUpDto
				{
					IsConfirmed = false,
					Message = "The check-up is already confirmed."
				};
			booking.Status = BookingStatus.Confirmed;
			await _context.SaveChangesAsync();
			return new ConfirmCheckUpDto
			{
				IsConfirmed = true,
				Message = "Check-up confirmed successfully."
			};
		}

		public async Task<AuthModel> LoginDoctorAsync(LoginDTO loginDTO)
		{
			return  await _auth.OnLoginAsync(loginDTO);
		}
		public async Task<IEnumerable<DoctorBookingResponseDto>> GetAllHisBoohingAsync(int doctorId, string searchBy, int pageSize = 10, int pageNumber = 1)
		{
			var query = _context.Bookings
				.Include(b => b.Time)
				.Include(b => b.Day)
				.Include(b => b.Patient)
				.ThenInclude(p => p.User)
				.Where(b => b.DoctorId == doctorId);
			if (!string.IsNullOrEmpty(searchBy))
			{
				query = query.Where(b =>
					b.Patient.User.FirstName.ToLower().Contains(searchBy.ToLower()) ||
					b.Patient.User.LastName.ToLower().Contains(searchBy.ToLower()) ||
					b.Patient.User.Email.ToLower().Contains(searchBy.ToLower()) ||
					b.Patient.User.Gender.ToString().ToLower().Contains(searchBy.ToLower()) ||
					b.Day.Day.ToString().ToLower().Contains(searchBy.ToLower()) ||
					b.Time.Time.ToString().ToLower().Contains(searchBy.ToLower())
				);
			}
			query = query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize);
			var bookings = await query.ToListAsync();
			var result = bookings.Select(b => new DoctorBookingResponseDto
			{
				PatientName = $"{b.Patient.User.FirstName} {b.Patient.User.LastName}",
				PatientEmail = b.Patient.User.Email,
				PatientGender = b.Patient.User.Gender.ToString(),
				PatientImage = b.Patient.User.Image,
				PatientPhone = b.Patient.User.Phone,
				PatientAge = CalculateAge(b.Patient.User.DateOfBirth)
			}).ToList();
			return result;
		}
		public async Task<UpdateAppointmentResponseDto> UpdateAppointmentAsync(UpdateAppointmentTimeRequest updateAppointment)
		{
			var appointment = await _context.Appointments
				.Include(a => a.Days)
					.ThenInclude(d => d.Times)
				.FirstOrDefaultAsync(a => a.Id == updateAppointment.AppointmentId);
			if (appointment == null)
				return new UpdateAppointmentResponseDto
				{
					IsUpdateed = false,
					Message = "Appointment not found."
				};
			var dayToUpdate = appointment.Days.FirstOrDefault(d => d.Id == updateAppointment.DayId);
			var timeToUpdate = dayToUpdate?.Times.FirstOrDefault(t => t.Id == updateAppointment.TimeId);
			if (dayToUpdate is null || timeToUpdate is null)
				return new UpdateAppointmentResponseDto
				{
					IsUpdateed = false,
					Message = "Day or time not found."
				};
			// Check if the new time is already booked
			var isTimeBooked = await _context.Bookings
				.Include(b => b.Day)
				.Include(b => b.Time)
				.AnyAsync(b =>
					b.DoctorId == appointment.DoctorId &&
					b.DayId == updateAppointment.DayId &&
					b.TimeId == updateAppointment.TimeId);
			if (isTimeBooked)
				return new UpdateAppointmentResponseDto
				{
					IsUpdateed = false,
					Message = "The selected time is already booked."
				};
			// Update the time
			timeToUpdate.Time = TimeSpan.Parse(updateAppointment.NewTime);
			await _context.SaveChangesAsync();

			return new UpdateAppointmentResponseDto
			{
				IsUpdateed = true,
				Message = "Time updated successfully."
			};
		}
		public async Task<DeleteAppointmentResponseDto> DeleteAppointmentAsync(DeleteTimeRequestDto deleteAppointment)
		{
			// Get the appointment, day, and time to delete
			var appointment = await _context.Appointments
				.Include(a => a.Days)
					.ThenInclude(d => d.Times)
				.FirstOrDefaultAsync(a => a.Id == deleteAppointment.AppointmentId);
			if (appointment is null)
				return new DeleteAppointmentResponseDto
				{
					IsDeleted = false,
					Message = "Appointment not found."
				};

			// Check if the day and time to delete exist
			var dayToDelete = appointment.Days.FirstOrDefault(d => d.Id == deleteAppointment.DayId);
			var timeToDelete = dayToDelete?.Times.FirstOrDefault(t => t.Id == deleteAppointment.TimeId);
			if (dayToDelete is null || timeToDelete is null)
				return new DeleteAppointmentResponseDto
				{
					IsDeleted = false,
					Message = "Day or time not found."
				};

			// Check if the time is already booked
			var isTimeBooked = await _context.Bookings
				.AnyAsync(b =>
					b.DoctorId == appointment.DoctorId &&
					b.DayId == deleteAppointment.DayId &&
					b.TimeId == deleteAppointment.TimeId);
			if (isTimeBooked)
				return new DeleteAppointmentResponseDto
				{
					IsDeleted = false,
					Message = "The selected time is already booked. It cannot be deleted."
				};
			// Delete the time
			dayToDelete.Times.Remove(timeToDelete);
			await _context.SaveChangesAsync();

			return new DeleteAppointmentResponseDto
			{
				IsDeleted = true,
				Message = "Time deleted successfully."
			};
		}
		private int CalculateAge(DateTime birthday)
		{
			var today = DateTime.Today;
			var age = today.Year - birthday.Year;

			if (birthday.Date > today.AddYears(-age))
			{
				age--;
			}

			return age;
		}
	}
}

