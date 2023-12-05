using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Vezeeta.Core.Helpers
{
	public class UploadHelpers
	{
		public const string PatientImagePath  = "images\\patients";
		public const string DoctorImagePath  = "images\\doctors";
		public const string AllowedExtensions = ".jpg,.jpeg,.png";
		public const int MaxFileSizeInMB = 1;
		public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;

		private readonly IWebHostEnvironment _webHostEnvironment;
		private string _imagesPath = string.Empty;
		public UploadHelpers(IWebHostEnvironment webHostEnvironment, string imagesPath)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public static string UploadFile(IFormFile file, string path)
		{

			//create a new name for the image
			var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			var fileName = $"{timeStamp}{Path.GetExtension(file.FileName)}";
			//the image path
			var relativePath = Path.Combine("wwwroot", path, fileName);
			using (var stream = new FileStream(relativePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			return relativePath;
		}
	}
}
