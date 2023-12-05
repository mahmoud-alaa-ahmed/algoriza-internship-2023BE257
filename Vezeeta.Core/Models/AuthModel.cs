namespace Vezeeta.Core.Models
{
	public class AuthModel
	{
		public string? Message { get; set; }
		public bool? IsAuthenticated { get; set; }
		public string? UserName { get; set; }
		public string? Id { get; set; }
		public string? Email { get; set; }
		public string? Token { get; set; }
		public List<string>? Roles { get; set; }
		public DateTime? ExpiresOn { get; set; }
	}
}
