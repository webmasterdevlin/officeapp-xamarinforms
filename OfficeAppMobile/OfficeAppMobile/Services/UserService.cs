using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JWT;
using JWT.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeApp.Helpers;
using OfficeAppMobile.Models;

namespace OfficeAppMobile.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<bool> SignupAsync(User user)
        {
            var content = JsonConvert.SerializeObject(user);

            var response = await _client.PostAsync(BaseUrl.SetSignupUrl(), new StringContent(content, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(User user)
        {
            string content = JsonConvert.SerializeObject(user);

            using (var response = await _client.PostAsync(BaseUrl.SetLoginUrl(),
                new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized) return false;

                if (response.IsSuccessStatusCode)
                {
                    await SetJwtToken(response);
                    await SetJwtExpirationDate(response);
                    return response.IsSuccessStatusCode;
                }

                return false;
            }
        }

        private static async Task SetJwtToken(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            UserToken userToken = JsonConvert.DeserializeObject<UserToken>(stringResponse);
            Settings.Jwt = userToken.Token;
        }

        private static async Task SetJwtExpirationDate(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            var decoded = new JwtBuilder().Decode(stringResponse);
            var userExp = JsonConvert.DeserializeObject<UserExp>(decoded);
            Settings.JwtExpirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(userExp.Exp)).DateTime;
        }
    }

    // ReSharper disable once ArrangeTypeModifiers
    internal class UserToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    internal class UserExp
    {
        [JsonProperty("exp")]
        public string Exp { get; set; }
    }
}