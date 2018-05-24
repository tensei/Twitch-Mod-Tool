using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Twitch_Mod_Tool.Services
{
    public class OverrustlelogsService
    {
        private readonly HttpClient _client;
        private readonly string _urlBase = "https://overrustlelogs.net";
        private readonly string _urlApiBase = "https://overrustlelogs.net/api/v1";
        public OverrustlelogsService()
        {
            _client = new HttpClient();
        }

        public async Task<string> GetUserlogs(string username, string channel)
        {
            var url = $"{_urlBase}/{channel} chatlog/{DateTime.Now:MMMM yyyy}/userlogs/{username}.txt";
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
