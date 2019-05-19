using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OfficeAppMobile.Models;
using OfficeAppMobile.Utils;

namespace OfficeAppMobile.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<string> SendGetAsync()
        {
            _client.AddAuthBearer();
            return await _client.GetStringAsync(BaseUrl.SetDepartmentUrl());
        }

        public async Task<HttpResponseMessage> SendPostAsync(string content)
        {
            _client.AddAuthBearer();
            using (HttpResponseMessage response = await _client.PostAsync(BaseUrl.SetDepartmentUrl(),
                new StringContent(content, Encoding.UTF8, "application/json")))

                return response;
        }

        public async Task<HttpResponseMessage> SendPutAsync(Department department, string content)
        {
            _client.AddAuthBearer();
            using (var response = await _client.PutAsync($"{BaseUrl.SetDepartmentUrl()}/{department.Id}",
                new StringContent(content, Encoding.UTF8, "application/json")))

                return response;
        }

        public async Task SendDeleteAsync(string id)
        {
            _client.AddAuthBearer();
            await _client.DeleteAsync($"{BaseUrl.SetDepartmentUrl()}/{id}");
        }
    }
}