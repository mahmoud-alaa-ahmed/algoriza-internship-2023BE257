using Vezeeta.Core.Enums;

namespace Vezeeta.Core.Models
{
	public class Discount:BaseEntity
	{
		public string DiscountCode { get; set; } = string.Empty;
		public int NumberOfRequests { get; set; }
		public DiscountType DiscountType { get; set; }
		public double Value { get; set; }
        public bool IsActive { get; set; }
    }
}
