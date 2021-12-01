using System.Text.Json.Serialization;

namespace Domain.types
{
    public interface ILoginResponse
    {
        [JsonPropertyName("token")]
        string Token { get; set; }
        [JsonPropertyName("firstName")]
        string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        string LastName { get; set; }
        
    }
}