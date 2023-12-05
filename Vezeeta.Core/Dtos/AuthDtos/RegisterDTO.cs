using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vezeeta.Core.Attributes;
using Vezeeta.Core.Enums;
using Vezeeta.Core.Helpers;

namespace Vezeeta.Core.Dtos.AuthDtos
{
	public class RegisterDTO
	{
		[Required, MaxLength(20)]
		public string FirstName { get; set; } = string.Empty;

		[Required, MaxLength(20)]
		public string LastName { get; set; } = string.Empty;

		[Required, MaxLength(30)]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public Gender Gender { get; set; }

		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		[AllowedExtensions(UploadHelpers.AllowedExtensions)]
		[MaxFileSize(UploadHelpers.MaxFileSizeInBytes)]
		public IFormFile? Image { get; set; }

		[Required, MaxLength(12)]
		public string Phone { get; set; } = string.Empty;

		public int? SpecializationId { get; set; }

		[Required, MaxLength(50)]
		public string Password { get; set; } = string.Empty;

		[Required, MaxLength(50)]
		[Compare("Password", ErrorMessage = "Password Not Match")]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required]
		public string UserType { get; set; } = string.Empty;
	}
}
