using System.Text.Json.Serialization;
using Domain.types;

namespace Domain
{
    public class Login: ILogin
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = "";
        [JsonPropertyName("Password")]
        public string Password { get; set; } = "";
    }
}