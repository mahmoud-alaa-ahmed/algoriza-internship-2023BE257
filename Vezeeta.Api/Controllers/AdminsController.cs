using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.ServiceInterfaces;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = UserRole.Admin)]
	public class AdminsController : ControllerBase
	{
		private readonly IAdminService _adminService;

		public AdminsController(IAdminService adminService)
		{
			_adminService = adminService;
		}

		[HttpGet("NumOfDoctors")]
		public async Task<IActionResult> NumOfDoctors()
		{
			try
			{
				return Ok(await _adminService.NumOfDoctors());
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("NumOfPatients")]
		public async Task<IActionResult> NumOfPatients()
		{
			try
			{
				return Ok(await _adminService.NumOfPatients());
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}

		}

		[HttpGet("NumOfRequests")]
		public async Task<IActionResult> NumOfRequests()
		{
			try
			{
				var res = await _adminService.NumOfRequests();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
			
		}

		[HttpGet("TopFiveSpecializations")]
		public async Task<IActionResult> TopFiveSpecializations()
		{
			try
			{
				var res = await _adminService.TopFiveSpecializations();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("TopTenDoctors")]
		public async Task<IActionResult> TopTenDoctors()
		{
			try
			{
				var res = await _adminService.TopTenDoctors();
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("GetAllDoctors")]
		public async Task<IActionResult> GetAllDoctors([FromQuery] string? Name, [FromQuery] int pageSize = 15, [FromQuery] int pageNumber = 1)
		{
			try
			{
				if(Name is not null)
				{
					var result = await _adminService.GetAllDoctors(d => d.User.FirstName.ToLower().Contains(Name.ToLower()), pageSize, pageNumber);
					return Ok(result);
				}
				var res = await _adminService.GetAllDoctors(null, pageSize, pageNumber);
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("GetDoctorById/{id}")]
		public async Task<IActionResult> GetDoctorById(int id)
		{
			try
			{
				var res = await _adminService.GetDoctorById(id);
				if (res is null)
					return NotFound($"No Doctor Found With Id: '{id}'");
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpPost("AddDoctor")]
		public async Task<IActionResult> AddDoctor([FromForm] RegisterDTO registerDTO)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);
				if (registerDTO.SpecializationId is null)
					return BadRequest("Please Enter Specialization Id");
				if ((await _adminService.GetSpecializationById((int)registerDTO.SpecializationId)) is null)
					return NotFound($"Not Found Specialization Id: '{registerDTO.SpecializationId}'");
				if (registerDTO.Image is null)
					return BadRequest("Please Enter Image");
				var createdDoctor = await _adminService.AddDoctor(registerDTO);
				if (createdDoctor.IsAuthenticated ==false)
					return BadRequest(false);
				return Ok(createdDoctor);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpPut("EditDoctor")]
		public async Task<IActionResult> EditDoctor([FromForm] EditDoctorDto registerDTO)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				if (registerDTO.SpecializationId is null)
					return BadRequest("Please Enter Specialization Id");
				if ((await _adminService.GetSpecializationById((int)registerDTO.SpecializationId)) is null)
					return NotFound($"Not Found Specialization Id: '{registerDTO.SpecializationId}'");
				if (registerDTO.Image is null)
					return BadRequest("Please Enter Image");
				var createdDoctor = await _adminService.EditDoctor(registerDTO);
				if (createdDoctor is null)
					return BadRequest(false);
				return Ok(createdDoctor);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpDelete("DeleteDoctor/{id}")]
		public async Task<IActionResult> DeleteDoctor(int id)
		{
			try
			{
				var deletedDoctor = await _adminService.DeleteDoctor(id);
				if(deletedDoctor is null)
					return BadRequest(false);
				return Ok(true);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("GetAllPatients")]
		public async Task<IActionResult> GetAllPatients([FromQuery] string? Name, [FromQuery] int pageSize = 15, [FromQuery] int pageNumber = 1)
		{
			try
			{
				if (Name is not null)
				{
					var result = await _adminService.GetAllPatients(d => d.User.FirstName.ToLower().Contains(Name.ToLower()), pageSize, pageNumber);
					return Ok(result);
				}
				var res = await _adminService.GetAllPatients(null, pageSize, pageNumber);
				return Ok(res);

			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("GetPatientById/{id}")]
		public async Task<IActionResult> GetPatientById(int id)
		{
			try
			{
				var res = await _adminService.GetPatientById(id);
				if (res is null)
					return NotFound($"No Patient Found With Id: '{id}'");
				return Ok(res);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpPost("AddCoupon")]
		public async Task<IActionResult> AddCoupon([FromForm] DiscountDto discount)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				var createdCoupon = await _adminService.AddDiscount(discount);
				if (createdCoupon is null)
					return BadRequest(false);
				return Ok(true);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpPut("UpdateCoupon")]
		public async Task<IActionResult> UpdateCoupon([FromForm] Discount discount)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				var updatedCoupon = await _adminService.UpdateDiscount(discount);
				if (updatedCoupon is null)
					return BadRequest(false);
				return Ok(true);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpDelete("DeleteCoupon/{id}")]
		public async Task<IActionResult> DeleteCoupon(int id)
		{
			try
			{
				var deletedCoupon = await _adminService.DeleteDiscount(id);
				if (deletedCoupon is null)
					return BadRequest(false);
				return Ok(true);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpPost("DeactivateCoupon/{id}")]
		public async Task<IActionResult> DeactivateCoupon(int id)
		{
			try
			{
				var eactivateCoupon = await _adminService.DeactivteDiscount(id);
				if (eactivateCoupon is null)
					return BadRequest(false);
				return Ok(true);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
