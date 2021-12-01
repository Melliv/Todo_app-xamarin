using System;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Register
    {
        [JsonPropertyName("firstName")] 
        public String FirstName { get; set; } = "";
        [JsonPropertyName("lastName")]
        public String LastName { get; set; } = "";
        [JsonPropertyName("email")]
        public String Email { get; set; } = "";
        [JsonPropertyName("password")]
        public String Password { get; set; } = "";
    }
}