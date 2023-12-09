using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Dtos.PatientDto;
using Vezeeta.Core.Models;
using Vezeeta.Core.ServiceInterfaces;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = UserRole.Patient)]
	public class PatientsController : ControllerBase
	{
		private readonly IPatientService _patientService;
		public PatientsController(IPatientService patientService)
		{
			_patientService = patientService;
		}
		[HttpGet("CancelHisBooking/{id}")]
		public async Task<IActionResult> CancelBookingAsync(int id)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).ToString();
				if (userId == null)
					return BadRequest("User not found.");
				var res= await _patientService.CancelBookingAsync(userId, id);
				if (res.IsCanceled == false)
					return BadRequest(res);
				return Ok(res);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}	
		}
		[AllowAnonymous]
		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromForm]RegisterDTO dTO)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				var res = await _patientService.RegisterAsync(dTO);
				if(res.IsAuthenticated == false)
					return BadRequest(res);
				return Ok(res);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				var res = await _patientService.LoginAsync(loginDTO);
				if (res.IsAuthenticated == false)
					return BadRequest(false);
				return Ok(res);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpGet("GetAllHisBooking")]
		public async Task<IActionResult> GetAllHisBooking()
		{
			try
			{
				var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).ToString();
				if (userId == null)
					return BadRequest("User not found.");
				var res = await _patientService.GetAllHisBooking(userId);
				if(res is null)
					return BadRequest(false);
				return Ok(res);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpPost("AddBooking")]
		public async Task<IActionResult> AddBooking([FromForm]AddBookingDto dto)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).ToString();
				if (userId == null)
					return BadRequest("User not found.");
				var res = await _patientService.AddBooking(dto, userId);
				if(res.IsCreated == false) 
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
