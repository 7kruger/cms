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
        private static string token = "dropbox_token";
        public async Task<string> UploadFileAsync(IFormFile file, string name)
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
                    var txurl = tx.Result.Url;
                    url = txurl.Replace("dl=0", "raw=1");
                    return url;
                }
            }
        }
    }
}
