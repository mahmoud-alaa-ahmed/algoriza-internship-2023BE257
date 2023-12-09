using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.DoctorDtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.ServiceInterfaces;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = UserRole.Doctor)]
	public class DoctorsController : ControllerBase
	{
        private readonly IDoctorService doctorService;
		public DoctorsController(IDoctorService doctorService)
		{
			this.doctorService = doctorService;
		}
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);
				var res = await doctorService.LoginDoctorAsync(loginDTO);
				if(res.IsAuthenticated == false)
					return BadRequest(false);
				return Ok(res);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpGet("GetAllBookings")]
		public async Task<IActionResult> GetAllBookings([FromQuery] int doctorId,[FromQuery] string searchBy = null,[FromQuery] int pageSize = 10,[FromQuery] int pageNumber = 1)
		{
			try
			{
				var bookings = await doctorService.GetAllHisBoohingAsync(doctorId, searchBy, pageSize, pageNumber);
				return Ok(bookings);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpPost("AddAppointment")]
		public async Task<IActionResult> AddAppointment(AppointmentDto dto)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);
				 var createdApp= await doctorService.AddAppointmentAsync(dto);
				if(createdApp is null)
					return BadRequest(false);
				return Ok(true);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpPut("EditAppointment")]
		public async Task<IActionResult> EditAppointment( [FromForm] UpdateAppointmentTimeRequest dto)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);
				var res = await doctorService.UpdateAppointmentAsync(dto);
				if(res.IsUpdateed == false)
					return BadRequest(res);
				return Ok(res);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpDelete("DeleteAppointment")]
		public async Task<IActionResult> DeleteAppointment([FromForm] DeleteTimeRequestDto dto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				var res = await doctorService.DeleteAppointmentAsync(dto);
				if(res.IsDeleted == false)
					return BadRequest(false);
				return Ok(res);

			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
