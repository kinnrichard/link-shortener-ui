using System.Text;
using System.Security.Cryptography;

namespace link_shortener_ui.Services
{
    public class UrlShorteningService
    {
        private readonly string domain;
        public UrlShorteningService(IConfiguration configuration)
        {
            domain = configuration["ShortUrlDomain"] ?? "https://sho.rt";
        }
        public string CreateShortUrl(string url)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(url));

                var base64Hash = Convert.ToBase64String(bytes).Substring(0, 6);

                base64Hash = base64Hash.Replace("/", "-").Replace("+", "_");

                return $"{domain}/{base64Hash}";
            }
        }
    }
}
