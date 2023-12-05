﻿using System.ComponentModel.DataAnnotations.Schema;
namespace Vezeeta.Core.Models
{
	public class Patient:BaseEntity
	{
		[ForeignKey(nameof(User))]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; } = default!;
	}
}
