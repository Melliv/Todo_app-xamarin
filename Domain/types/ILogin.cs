using System.Text.Json.Serialization;

namespace Domain.types
{
    public interface ILogin
    {
        [JsonPropertyName("email")]
        string Email { get; set; }

        [JsonPropertyName("password")]
        string Password { get; set; }
    }
}