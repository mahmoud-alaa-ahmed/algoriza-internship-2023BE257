using System.ComponentModel.DataAnnotations;

namespace Vezeeta.Core.Dtos.AuthDtos
{
	public class LoginDTO
	{
		[Required, MaxLength(80)]
		public string Email { get; set; } = string.Empty;
		[Required, MaxLength(50)]
		public string Password { get; set; } = string.Empty;
	}
}
