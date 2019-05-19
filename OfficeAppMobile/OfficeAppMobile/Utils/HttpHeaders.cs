using System.Net.Http;
using OfficeApp.Helpers;

namespace OfficeAppMobile.Utils
{
    public static class HttpHeaders
    {
        
        public static void AddAuthBearer(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Settings.Jwt}");
        }        
    }
}