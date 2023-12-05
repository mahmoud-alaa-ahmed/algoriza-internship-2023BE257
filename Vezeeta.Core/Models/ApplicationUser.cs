using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Vezeeta.Core.Enums;

namespace Vezeeta.Core.Models
{
    public class ApplicationUser: IdentityUser
	{
		[Required, MaxLength(20)]
		public string FirstName { get; set; } = string.Empty;

		[Required, MaxLength(20)]
		public string LastName { get; set; } = string.Empty;

		[Required, MaxLength(12)]
		public string Phone { get; set; } = string.Empty;
		public Gender Gender { get; set; }

		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		public string? Image { get; set; } = string.Empty;
		[Required]
		public string UserType { get; set; } = string.Empty;
    }
	
}
