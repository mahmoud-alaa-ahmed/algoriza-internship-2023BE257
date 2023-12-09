using System.Linq.Expressions;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Dtos.PatientDto;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.ServiceInterfaces
{
	public interface IAdminService
	{
		Task<int> NumOfDoctors();
		Task<int> NumOfPatients();
		Task<NumOfRequestsDto> NumOfRequests();
		Task<IEnumerable<TopFiveSpecializationsDto>> TopFiveSpecializations();
		Task<IEnumerable<TopTenDoctorsDto>> TopTenDoctors();
		Task<IEnumerable<GetAllDoctorResponseDto>> GetAllDoctors(Expression<Func<Doctor, bool>> search,int pageSize = 15, int pageNumber = 1);
		Task<GetDoctorByIdDto> GetDoctorById(int Id);
		Task<AuthModel> AddDoctor(RegisterDTO register);
		Task<Doctor> EditDoctor(EditDoctorDto editDoctor);
		Task<Doctor> DeleteDoctor(int id);
		Task<IEnumerable<GetAllPatientResponseDto>> GetAllPatients(Expression<Func<Patient, bool>> search = null, int pageSize = 15, int pageNumber = 1);

		Task<GetPatientByIdDto> GetPatientById(int Id);
		Task<Discount> AddDiscount(DiscountDto discount);
		Task<Discount> UpdateDiscount(Discount discount);
		Task<Discount> DeleteDiscount(int id);
		Task<Discount> DeactivteDiscount(int id);
		Task<Specialization> GetSpecializationById(int id);

	}
}
