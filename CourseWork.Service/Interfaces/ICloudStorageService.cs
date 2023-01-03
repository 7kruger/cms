using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface ICloudStorageService
	{
		Task<string> UploadAsync(IFormFile image, string folder, string srcId);
		Task<string> UpdateAsync(IFormFile image, string folder, string srcId);
		Task DeleteAsync(string folder, string srcId);
	}
}
