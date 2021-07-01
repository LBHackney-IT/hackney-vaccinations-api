using System.Text.Json.Serialization;

namespace HackneyVaccinationsApi.V1.Boundary.Requests
{
    public class ConfirmationRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("mobileNumber")]
        public string MobileNumber { get; set; }
    }
}
