using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminsController : ControllerBase
	{
		[HttpGet]
		public IActionResult NumOfDoctors()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult NumOfPations()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult NumOfRequests()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult TopFiveSpecializations()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult TopTenDoctors()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult GetAllDoctors()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult GetDoctorById()
		{
			return Ok(0);
		}

		[HttpPost]
		public IActionResult AddDoctor()
		{
			return Ok(0);
		}

		[HttpPut]
		public IActionResult EditDoctor()
		{
			return Ok(0);
		}

		[HttpDelete]
		public IActionResult DeleteDoctor()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult GetAllPatients()
		{
			return Ok(0);
		}

		[HttpGet]
		public IActionResult GetPatientById()
		{
			return Ok(0);
		}

		[HttpPost]
		public IActionResult AddSetting()
		{
			return Ok(0);
		}

		[HttpPut]
		public IActionResult UpdateSetting()
		{
			return Ok(0);
		}
		[HttpDelete]
		public IActionResult DeleteSetting()
		{
			return Ok(0);
		}

		[HttpPost]
		public IActionResult Deactivate
()
		{
			return Ok(0);
		}
	}
}
