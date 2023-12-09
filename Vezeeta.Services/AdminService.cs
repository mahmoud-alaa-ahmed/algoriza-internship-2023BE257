using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Dtos.PatientDto;
using Vezeeta.Core.Enums;
using Vezeeta.Core.Helpers;
using Vezeeta.Core.Models;
using Vezeeta.Core.RepositoryInterfaces;
using Vezeeta.Core.ServiceInterfaces;

namespace Vezeeta.Services
{
	[Authorize(Roles = UserRole.Admin)]
	public class AdminService :  IAdminService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuth _auth;
		private readonly UserManager<ApplicationUser> _userManager;
		public AdminService(IUnitOfWork unitOfWork, IAuth auth, UserManager<ApplicationUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_auth = auth;
			_userManager = userManager;
		}
		public async Task<Discount> AddDiscount(DiscountDto discount)
		{
			Discount dis = new Discount();
			dis.DiscountCode = discount.DiscountCode;
			dis.Value = discount.Value;
			dis.NumberOfRequests = discount.NumberOfRequests;
			dis.IsActive = discount.IsActive;
			dis.DiscountType = discount.DiscountType;

			var res = await _unitOfWork.Discount.AddAsync(dis);
			await _unitOfWork.SaveDataAsync();
			if(res is not null)
			 return res;

			return null;
		}

		public async Task<AuthModel> AddDoctor(RegisterDTO register)
		{
			var res = await _auth.OnRegisterAsync(register, "doctors", UserRole.Doctor);
			if(res.IsAuthenticated == true)
			{
				Doctor doctor = new Doctor();
				doctor.UserId = res.Id;
				doctor.SpecializationId = (int)register.SpecializationId;
				await _unitOfWork.Doctor.AddAsync(doctor);
				await _unitOfWork.SaveDataAsync();		
			}
			return res;
		}

		public async Task<Discount> DeactivteDiscount(int id)
		{
			var discount = await _unitOfWork.Discount.GetByIdAsync(d => d.Id == id);
			if(discount is not null)
			{
				discount.IsActive = false;
				_unitOfWork.Discount.Edit(discount);
				await _unitOfWork.SaveDataAsync();
				return discount;
			}
			return null;
		}

		public async Task<Discount> DeleteDiscount(int id)
		{
			var discount = await _unitOfWork.Discount.GetByIdAsync(d => d.Id == id);
			if (discount is not null)
			{
				_unitOfWork.Discount.Delete(discount);
				await _unitOfWork.SaveDataAsync();
				return discount;
			}
			return null;
		}

		public async Task<Doctor> DeleteDoctor(int id)
		{
			Doctor doctor = await _unitOfWork.Doctor.GetByIdAsync(d => d.Id == id);
			if (doctor is not null)
			{
				var user = await _auth.FindUserAsync(doctor.UserId);
				_unitOfWork.Doctor.Delete(doctor);
				await _unitOfWork.SaveDataAsync();
				await _userManager.DeleteAsync(user);

				return doctor;
			}
			return null;

		}

		public async Task<Doctor> EditDoctor(EditDoctorDto editDoctor)
		{
			Doctor doctor = await _unitOfWork.Doctor.GetByIdAsync(d => d.Id==editDoctor.Id);
			if(doctor is not null)
			{
				var user = await _auth.FindUserAsync(doctor.UserId);
				user.FirstName = editDoctor.FirstName;
				user.LastName = editDoctor.LastName;
				user.Email = editDoctor.Email;
				user.Phone = editDoctor.Phone;
				user.Gender = editDoctor.Gender;
				if (editDoctor.Image != null)
					user.Image = UploadHelpers.UploadFile(editDoctor.Image, "doctors");
				else
					user.Image = "images/doctors/doctor-not-found.jpg";
				
				var result = await _userManager.UpdateAsync(user);
				if(result.Succeeded)
				{
					doctor.SpecializationId = (int)editDoctor.SpecializationId;
					_unitOfWork.Doctor.Edit(doctor);
					await _unitOfWork.SaveDataAsync();
					return doctor;

				}
			}
			return null;
		}

		public async Task<IEnumerable<GetAllDoctorResponseDto>> GetAllDoctors(Expression<Func<Doctor, bool>> search = null, int pageSize = 15, int pageNumber = 1)
		{
			var doctors = await _unitOfWork.Doctor.GetAllAsync(new string[]{ "User", "Specialization" },search, pageSize, pageNumber);
			if(doctors is not null)
			{
				var doctorDto = doctors.Select(d => new GetAllDoctorResponseDto
				{
					FullName = $"{d.User.FirstName} {d.User.LastName}",
					Image = d.User.Image,
					Specialize = d.Specialization.Name,
					Email = d.User.Email,
					Phone = d.User.Phone,
					Gender = d.User.Gender.ToString()
				});
				return doctorDto;
			}
			return null;
		}

		public async Task<IEnumerable<GetAllPatientResponseDto>> GetAllPatients(Expression<Func<Patient, bool>> search, int pageSize = 15, int pageNumber = 1)
		{
			var Patients = await _unitOfWork.Patient.GetAllAsync(new[] { "User" }, search, pageSize, pageNumber);
			if(Patients is not null)
			{
				var patientDto = Patients.Select(p => new GetAllPatientResponseDto
				{
					FullName = $"{p.User.FirstName} {p.User.LastName}",
					Image = p.User.Image,
					Email = p.User.Email,
					Phone = p.User.Phone,
					Gender = p.User.Gender.ToString(),
					DateOfBrith =p.User.DateOfBirth,
				});
				return patientDto;
			}
			return null;
		}

		public async Task<GetDoctorByIdDto> GetDoctorById(int id)
		{
			var doctor = await _unitOfWork.Doctor.GetByIdAsyncAsNoTracking(d => d.Id == id, new[] { "User", "Specialization" } );
			if(doctor is not null)
			{
				var doctorDto = new GetDoctorByIdDto
				{
					FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
					Image = doctor.User.Image,
					Specialize = doctor.Specialization.Name,
					Email = doctor.User.Email,
					Phone = doctor.User.Phone,
					Gender = doctor.User.Gender.ToString(),
					DateOfBrith=doctor.User.DateOfBirth,
				};
				return doctorDto;
			}
			return null;
		}

		public async Task<GetPatientByIdDto> GetPatientById(int Id)
		{
			var p = await _unitOfWork.Patient.GetByIdAsyncAsNoTracking(p=> p.Id == Id,new string[] { "User", });
			if(p is not null)
			{
				var booking = await _unitOfWork.Booking.GetAllBookingAsync(Id);
				var requests = booking.Select(r => new RequestDto
				{
					DoctorImage = r.Doctor.User.Image,
					DoctorNmae = r.Doctor.User.FirstName,
					Specialize = r.Doctor.Specialization.Name,
					discoundCode = r.Discount.DiscountCode,
					Price = r.Price,

				}).ToList();
				if (requests is null)
					return null;

				var patientDto =  new GetPatientByIdDto
				{
					FullName = $"{p.User.FirstName} {p.User.LastName}",
					Image = p.User.Image,
					Email = p.User.Email,
					Phone = p.User.Phone,
					Gender = p.User.Gender.ToString(),
					DateOfBrith = p.User.DateOfBirth,
					Requests = requests
				};
				return patientDto;
			}
			return null;
		}

		public async Task<int> NumOfDoctors()
		{
			var doctors = await _unitOfWork.Doctor.GetAsync();
			if (doctors is not null)
				return doctors.Count();
			return 0;
		}

		public async Task<int> NumOfPatients()
		{
			var patients = await _unitOfWork.Patient.GetAsync();
			if(patients is not null)
				return patients.Count();
			return 0;
		}

		public async Task<NumOfRequestsDto> NumOfRequests()
		{
			var res = new NumOfRequestsDto
			{
				CancelledRequests = (await _unitOfWork.Booking.GetAllAsync(null, b => b.Status == BookingStatus.Canceled)).Count(),
				PendingRequests = (await _unitOfWork.Booking.GetAllAsync(null, b => b.Status == BookingStatus.Pending)).Count(),
				CompletedRequests = (await _unitOfWork.Booking.GetAllAsync(null, b => b.Status == BookingStatus.Confirmed)).Count()
			};
			return res;
		}

		public async Task<IEnumerable<TopFiveSpecializationsDto>> TopFiveSpecializations()
		{
			return await _unitOfWork.Booking.TopFiveSpecializationsAsync();

		}

		public async Task<IEnumerable<TopTenDoctorsDto>> TopTenDoctors()
		{
			return await _unitOfWork.Booking.TopTenDoctorsAsync();
		}

		public async Task<Discount> UpdateDiscount(Discount discount)
		{
			var disModel =await _unitOfWork.Discount.GetByIdAsync(d => d.Id == discount.Id);
			if(disModel is not null)
			{
				disModel.DiscountCode = discount.DiscountCode;
				disModel.IsActive = discount.IsActive;
				disModel.Value = discount.Value;
				disModel.NumberOfRequests = discount.NumberOfRequests;
				disModel.DiscountType = discount.DiscountType;

				_unitOfWork.Discount.Edit(disModel);
				await _unitOfWork.SaveDataAsync();
				return disModel;
			}
			return null;
		}
		public async Task<Specialization> GetSpecializationById(int id)
		{
			return await _unitOfWork.Specialization.GetByIdAsyncAsNoTracking(s => s.Id == id);
		}
	}
}
