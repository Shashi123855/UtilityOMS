namespace UtilityOMS.Web.Services
{
    public class CrewDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LeadTechnician { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ActiveOutagesCount { get; set; }
    }

    public class CreateCrewDto
    {
        public string Name { get; set; } = string.Empty;
        public string LeadTechnician { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class UpdateCrewDto
    {
        public string Name { get; set; } = string.Empty;
        public string LeadTechnician { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    public class CrewClientService
    {
        private readonly HttpClient _http;

        public CrewClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CrewDto>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<CrewDto>>("api/crew")
               ?? new List<CrewDto>();

        public async Task<bool> CreateAsync(CreateCrewDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/crew", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCrewDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/crew/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/crew/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
