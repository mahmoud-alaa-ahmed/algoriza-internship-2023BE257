
using System.ComponentModel.DataAnnotations.Schema;


namespace Vezeeta.Core.Models
{
	public class Doctor :BaseEntity
	{
		[ForeignKey(nameof(User))]
        public string UserId { get; set; }
		public ApplicationUser User { get; set; } = default!;

        [ForeignKey(nameof(Specialization))]
        public int SpecializationId { get; set; }
		public Specialization Specialization { get; set; } = default!;

	}
}
