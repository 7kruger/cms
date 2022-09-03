using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;

namespace CourseWork.Services
{
    public class DropboxService
    {
        private readonly string token;

        public DropboxService()
		{
            var path = Environment.CurrentDirectory + "/wwwroot/files/token.txt";
			using (var reader = new StreamReader(path))
			{
                token = reader.ReadToEnd();
			}
		}

        public async Task<string> UploadFileAsync(IFormFile file, string name)
        {
			try
			{
                using (var dbx = new DropboxClient(token))
                {
                    string folder = "/Public";
                    string filename = name;
                    string url = "";
                    using (var mem = file.OpenReadStream())
                    {
                        var updated = dbx.Files.UploadAsync($"{folder}/{filename}", WriteMode.Overwrite.Instance, body: mem);
                        updated.Wait();
                        var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync($"{folder}/{filename}");
                        tx.Wait();
                        return tx.Result.Url.Replace("dl=0", "raw=1");
                    }
                }
            }
			catch (Exception)
			{
                return string.Empty;
			}
        }
    }
}
