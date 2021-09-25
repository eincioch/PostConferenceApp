using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CertificateImageGenerator.Helpers
{
    public static class ImageDownloaderHelper
    {

        public static async Task<byte[]> DownloadPhoto(string uri) {

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetByteArrayAsync(uri);
            return response;
        }
    }
}
