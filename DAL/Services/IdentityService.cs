using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.types;

namespace DAL.Services
{
    public static class IdentityService
    {
        private const string BaseUrl = "https://taltech.akaver.com/api/v1/";

        public static async Task<IHttpResponse<ILoginResponse>> Login(ILogin loginData)
        {
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json");

            using (var client = new HttpClient())
            {
                var httpResponse = await client.PostAsync(BaseUrl + "/Account/Login", content);
                var responseBodyStr = await httpResponse.Content.ReadAsStringAsync();
                var responseBody =
                    System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(responseBodyStr);

                var response = new HttpResponse<ILoginResponse>
                {
                    Ok = httpResponse.IsSuccessStatusCode,
                    StatusCode = httpResponse.StatusCode,
                    Data = responseBody
                };

                return response;
            }
        }
        
        public static async Task<IHttpResponse<ILoginResponse>> Register(Register registerData)
        {
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(registerData),
                Encoding.UTF8,
                "application/json");

            using (var client = new HttpClient())
            {
                var httpResponse = await client.PostAsync(BaseUrl + "/Account/Register", content);
                var responseBodyStr = await httpResponse.Content.ReadAsStringAsync();
                var responseBody =
                    System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(responseBodyStr);

                var response = new HttpResponse<ILoginResponse>
                {
                    Ok = httpResponse.IsSuccessStatusCode,
                    StatusCode = httpResponse.StatusCode,
                    Data = responseBody
                };

                return response;
            }
        }
    }
}