using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.PatientDto;
using Vezeeta.Core.Enums;
using Vezeeta.Core.Models;
using Vezeeta.Core.RepositoryInterfaces;
using Vezeeta.Core.ServiceInterfaces;
using Vezeeta.Infrastructure.Data;

namespace Vezeeta.Services
{
	public class PatientService : IPatientService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuth _auth;
		private readonly ApplicationDbContext _context;
		public PatientService(IUnitOfWork unitOfWork, IAuth auth, ApplicationDbContext context)
		{
			this._unitOfWork = unitOfWork;
			_auth = auth;
			_context = context;
		}
		public async Task<IEnumerable<GettPatientAllHisBokingDto>> GetAllHisBooking(string userId)
		{
			var patient = await _context.Patients
				.Include(p  => p.User)
				.Include(p => p.Bookings)
				.ThenInclude(b => b.Doctor)
				.ThenInclude(d => d.Specialization)
				.Include(p => p.Bookings)
				.ThenInclude(d => d.Discount)
				.Where(p => p.UserId == userId)
				.FirstOrDefaultAsync();

			if (patient is null)
				return null;
			var bookings = patient.Bookings.Select(b => new GettPatientAllHisBokingDto
			{
				DoctorImage = b.Doctor.User.Image,
				DoctorName = $"{b.Doctor.User.FirstName} {b.Doctor.User.LastName}",
				Specialize = b.Doctor.Specialization.Name,
				Day = b.Day.Day.ToString(),
				Time = b.Time.Time.ToString(), // Adjust the time format as needed
				Price = b.Price,
				DiscountCode = b.Discount.DiscountCode,
				FinalPrice = b.FinalPrice,
				Status = b.Status.ToString()
			}).ToList();
			return bookings;
		}

		public async Task<AuthModel> LoginAsync(LoginDTO loginDTO)
		{
			return await _auth.OnLoginAsync(loginDTO);
		}

		public async Task<AuthModel> RegisterAsync(RegisterDTO registerDTO)
		{
			var res = await _auth.OnRegisterAsync(registerDTO, "patients",UserRole.Patient);
			if (res.IsAuthenticated == true)
			{
				Patient p = new Patient
				{
					UserId = res.Id,
				};
				await _unitOfWork.Patient.AddAsync(p);
				await _unitOfWork.SaveDataAsync();
			}
			return res;
		}

		public async Task<CancelBookingResponseDto> CancelBookingAsync(string userId, int bookingId)
		{
			var patient = await _unitOfWork.Patient.GetByIdAsync(p => p.UserId == userId,new string[] { "Bookings" });
			var booking = await _unitOfWork.Booking.GetByIdAsync(b => b.Id == bookingId);
			if (booking is null)
				return new CancelBookingResponseDto
				{
					IsCanceled = false,
					Message = $"Invaild Booking Id: {bookingId}"
				};
			if ( patient.Id == booking.PatientId)
			{
				booking.Status = Core.Enums.BookingStatus.Canceled;
				_unitOfWork.Booking.Edit(booking);
				await _unitOfWork.SaveDataAsync();
				return new CancelBookingResponseDto
				{
					IsCanceled= true,
					Message = "Booking successfully canceled."
				};
			}
			return new CancelBookingResponseDto
			{
				IsCanceled = false,
				Message = "Unable to cancel the booking. Please check the booking details."
			};
		}
		public async Task<BookingResponseDto> AddBooking(AddBookingDto dto,string userId)
		{
			var doctor = await _unitOfWork.Doctor.GetByIdAsyncAsNoTracking(d => d.Id == dto.DoctorId);
			if (doctor is null)
				return new BookingResponseDto
				{
					IsCreated = false,
					Message = "Doctor not found"
				};
			var appointment = await _unitOfWork.Appointment.GetByIdAsyncAsNoTracking(a => a.DoctorId == dto.DoctorId);
			if (appointment is null)
				return new BookingResponseDto
				{
					IsCreated = false,
					Message = "appointment not found"
				};
			var discount = await _unitOfWork.Discount.GetByIdAsyncAsNoTracking(d => d.Id == dto.DiscountId);

			if (discount is null)
				return new BookingResponseDto
				{
					IsCreated = false,
					Message = "Discount not found"
				};
			var day = await _unitOfWork.Day.GetByIdAsyncAsNoTracking(d => d.Id == dto.DayId);
			if (day is null)
				return new BookingResponseDto
				{
					IsCreated = false,
					Message = "Day not found"
				};
			var time = await _unitOfWork.Time.GetByIdAsyncAsNoTracking(d => d.Id == dto.TimeId);
			if (time is null)
				return new BookingResponseDto
				{
					IsCreated = false,
					Message = "Time not found"
				};
			var patient = await _unitOfWork.Patient.GetByIdAsyncAsNoTracking(p => p.UserId == userId);
			if (patient is null)
				return new BookingResponseDto
				{
					IsCreated = false,
					Message = "Patient not found"
				};
			Booking booking = new Booking();
			booking.Status = BookingStatus.Pending;
			booking.DoctorId = dto.DoctorId;
			booking.DiscountId = dto.DiscountId;
			booking.DayId = dto.DayId;
			booking.TimeId = dto.TimeId;
			booking.PatientId = patient.Id;
			booking.Price = appointment.Price;
			booking.FinalPrice = CalculatePrice(appointment.Price, discount.DiscountType, discount.Value);
			await _unitOfWork.Booking.AddAsync(booking);
			await _unitOfWork.SaveDataAsync();
			return new BookingResponseDto
			{
				IsCreated = true,
				Message = "Booking successfully added."
			};
		}
		private double CalculatePrice(double doctorPrice, DiscountType discountType,double Value)
		{
			double basePrice = doctorPrice;
			double discountedPrice;
			if (discountType == DiscountType.Percentage)
			{
				double discountPercentage = Value / 100.0;
				discountedPrice = basePrice * (1 - discountPercentage);
			}
			else
			{
				discountedPrice = basePrice - Value;
			}
			double finalPrice = Math.Max(discountedPrice, 0);

			return finalPrice;
		}
	}
}
