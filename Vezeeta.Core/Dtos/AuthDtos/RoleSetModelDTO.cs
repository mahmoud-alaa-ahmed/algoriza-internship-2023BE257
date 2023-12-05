using System.ComponentModel.DataAnnotations;

namespace Vezeeta.Core.Dtos.AuthDtos
{
	public class RoleSetModelDTO
	{
		[Required]
		public string Email { get; set; } = string.Empty;	
		[Required]
		public string Role { get; set; } = string.Empty;
	}
}
