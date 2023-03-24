using CourseWork.Service.Interfaces;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class DropboxService : ICloudStorageService
    {
        private const string imgnotfound = "/images/imgnotfound.jpg";

        private readonly string refreshToken;
		private readonly string appKey;
		private readonly string appSecret;

        public DropboxService(IConfiguration configuration)
		{
			refreshToken = configuration["Dropbox:RefreshToken"];
			appKey = configuration["Dropbox:AppKey"];
			appSecret = configuration["Dropbox:AppSecret"];
        }

		public async Task<string> UploadAsync(IFormFile image, string folder, string srcId)
		{
			try
			{
				if (image != null)
				{
					string extension = Path.GetExtension(image.FileName);
					if (extension == ".png" || extension == ".jpg")
					{
						return await UploadImage(image, folder, srcId);
					}
				}

				return imgnotfound;
			}
			catch (Exception)
			{
				return imgnotfound;
			}
		}

		public async Task<string> UpdateAsync(IFormFile image, string folder, string srcId)
		{
			try
			{
				string filename = $"img-{srcId}.jpg";

				var isExists = await IsExists(folder, filename);

				if (isExists)
				{
					await DeleteImage(folder, filename);
				}

				return await UploadImage(image, folder, srcId);
			}
			catch (Exception)
			{
				return imgnotfound;
			}
		}

		public async Task DeleteAsync(string folder, string srcId)
		{
			try
			{
				string filename = $"img-{srcId}.jpg";
				await DeleteImage(folder, filename);
			}
			catch (Exception)
			{

			}
		}

		private async Task<string> UploadImage(IFormFile image, string folder, string srcId)
		{
			using (var dbx = new DropboxClient(refreshToken, appKey, appSecret))
			{
				string filename = $"img-{srcId}.jpg";
				using (var mem = image.OpenReadStream())
				{
					await dbx.Files.UploadAsync($"{folder}/{filename}", WriteMode.Overwrite.Instance, body: mem);
					var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync($"{folder}/{filename}");
					tx.Wait();
					return tx.Result.Url.Replace("dl=0", "raw=1");
				}
			}
		}

		private async Task DeleteImage(string folder, string filename)
		{
			using (var dbx = new DropboxClient(refreshToken, appKey, appSecret))
			{
				await dbx.Files.DeleteAsync($"{folder}/{filename}");
			}
		}

		private async Task<bool> IsExists(string folder, string filename)
		{
			try
			{
				using (var dbx = new DropboxClient(refreshToken, appKey, appSecret))
				{
					var response = await dbx.Files.ListFolderAsync(folder);

					var result = response.Entries.FirstOrDefault(i => i.Name == filename);

					return result == null ? false : true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
