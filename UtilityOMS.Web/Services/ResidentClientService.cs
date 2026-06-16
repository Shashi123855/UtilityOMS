namespace UtilityOMS.Web.Services
{
    public class ResidentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool SmsOptIn { get; set; }
        public bool EmailOptIn { get; set; }
        public DateTime RegisteredAt { get; set; }
    }

    public class CreateResidentDto
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool SmsOptIn { get; set; } = true;
        public bool EmailOptIn { get; set; } = true;
    }
    public class UpdateResidentDto
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool SmsOptIn { get; set; } = true;
        public bool EmailOptIn { get; set; } = true;
    }

    public class ResidentClientService
    {
        private readonly HttpClient _http;

        public ResidentClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ResidentDto>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<ResidentDto>>("api/resident")
               ?? new List<ResidentDto>();

        public async Task<bool> CreateAsync(CreateResidentDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/resident", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateAsync(int id, UpdateResidentDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/resident/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/resident/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
