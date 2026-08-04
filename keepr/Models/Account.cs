using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace keepr.Models
{
    public class Profile
    {
        [JsonProperty("sub")]
        [JsonPropertyName("sub")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
    }
    public class Account : Profile
    {
        public string Email { get; set; }
    }
}