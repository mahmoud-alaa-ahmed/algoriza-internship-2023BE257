using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Vezeeta.Core.Helpers
{
	public class UploadHelpers
	{
		public const string AllowedExtensions = ".jpg,.jpeg,.png";
		public const int MaxFileSizeInMB = 1;
		public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
		public static string UploadFile(IFormFile file, string FolderName)
		{
			
			var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FolderName);
			
			var fileName = Guid.NewGuid() + file.FileName;
			
			var filePath = Path.Combine(FolderPath, fileName);
			
			var FileStream = new FileStream(filePath, FileMode.Create);
			
			file.CopyTo(FileStream);
			
			return Path.Combine($"images\\{FolderName}", fileName);
		}
		public static void DeleteFile(string folderName, string fileName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName, fileName);
			if (File.Exists(filePath))
				File.Delete(filePath);
		}
	}
}
