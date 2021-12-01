using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.types;
using Xamarin.Essentials;

namespace DAL.Services
{
    public static class BaseService
    {
        private const string BaseUrl = "https://taltech.akaver.com/api/v1/";

        private static HttpRequestMessage GetRequest(string apiEndPoint, HttpMethod method, Object dto)
        {
            var token = Preferences.Get("token", "no value :(");
            var requestMessage = new HttpRequestMessage
            {
                Method = method,
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(dto), 
                    Encoding.UTF8, 
                    "application/json"),
                RequestUri = new Uri(BaseUrl + apiEndPoint)
            };

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return requestMessage;
        }
        
        private static HttpRequestMessage GetRequestNoContent(string apiEndPoint, HttpMethod method)
        {
            var token = Preferences.Get("token", "no value :(");
            var requestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(BaseUrl + apiEndPoint)
            };

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return requestMessage;
        }


        public static async Task<IHttpResponse<TData>> Post<TData>(string apiEndPoint, TData dto)
        {
            var request = GetRequest(apiEndPoint, HttpMethod.Post, dto);
            
            using (var client = new HttpClient())
            {
                var httpResponse = await client.SendAsync(request);
                var responseBodyStr = await httpResponse.Content.ReadAsStringAsync();
                var responseBody =
                    System.Text.Json.JsonSerializer.Deserialize<TData>(responseBodyStr);

                var response = new HttpResponse<TData>
                {
                    Ok = httpResponse.IsSuccessStatusCode,
                    StatusCode = httpResponse.StatusCode,
                    Data = responseBody
                };

                return response;
            }
        }
        
        public static async Task<IHttpResponse<TData>> Put<TData>(string apiEndPoint, TData dto)
        {
            var request = GetRequest(apiEndPoint, HttpMethod.Put, dto);
            
            using (var client = new HttpClient())
            {
                var httpResponse = await client.SendAsync(request);
                var response = new HttpResponse<TData>
                {
                    Ok = httpResponse.IsSuccessStatusCode,
                    StatusCode = httpResponse.StatusCode,
                };

                return response;
            }
        }
        
        public static async Task<IHttpResponse<TData>> Delete<TData>(string apiEndPoint)
        {
            var request = GetRequestNoContent(apiEndPoint, HttpMethod.Delete);
            
            using (var client = new HttpClient())
            {
                var httpResponse = await client.SendAsync(request);
                var response = new HttpResponse<TData>
                {
                    Ok = httpResponse.IsSuccessStatusCode,
                    StatusCode = httpResponse.StatusCode,
                };

                return response;
            }
        }
        
        public static async Task<IHttpResponse<TData>> Get<TData>(string apiEndPoint)
        {
            var request = GetRequestNoContent(apiEndPoint, HttpMethod.Get);

            using (var client = new HttpClient())
            {
                var httpResponse = await client.SendAsync(request);
                var responseBodyStr = await httpResponse.Content.ReadAsStringAsync();
                var responseBody =
                    System.Text.Json.JsonSerializer.Deserialize<TData>(responseBodyStr);
                
                var response = new HttpResponse<TData>
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