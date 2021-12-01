using System.Text.Json.Serialization;
using Domain.types;

namespace Domain
{
    public class LoginResponse : ILoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
    }
}