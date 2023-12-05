using System.ComponentModel.DataAnnotations;


namespace Vezeeta.Core.Models
{
	public class BaseEntity
	{
		[Key]
        public int Id { get; set; }
    }
}
