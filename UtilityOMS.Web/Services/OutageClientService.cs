namespace UtilityOMS.Web.Services
{
    public class OutageDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Cause { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AffectedArea { get; set; } = string.Empty;
        public int AffectedCustomers { get; set; }
        public string ReportedBy { get; set; } = string.Empty;
        public int? AssignedCrewId { get; set; }
        public string? AssignedCrewName { get; set; }
    }

    public class CreateOutageDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Cause { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AffectedArea { get; set; } = string.Empty;
        public int AffectedCustomers { get; set; }
        public string ReportedBy { get; set; } = string.Empty;
    }

    public class UpdateOutageDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Status { get; set; }
        public int Cause { get; set; }
        public string AffectedArea { get; set; } = string.Empty;
        public int AffectedCustomers { get; set; }
        public int? AssignedCrewId { get; set; }
    }

    public class OutageClientService
    {
        private readonly HttpClient _http;

        public OutageClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OutageDto>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<OutageDto>>("api/outage")
               ?? new List<OutageDto>();

        public async Task<OutageDto?> GetByIdAsync(int id)
            => await _http.GetFromJsonAsync<OutageDto>($"api/outage/{id}");

        public async Task<bool> CreateAsync(CreateOutageDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/outage", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateOutageDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/outage/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/outage/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
