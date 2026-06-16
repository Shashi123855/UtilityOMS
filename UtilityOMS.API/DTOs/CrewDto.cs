using UtilityOMS.API.Models;

namespace UtilityOMS.API.DTOs
{
    // Used to READ crew data (GET)
    public class CrewResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LeadTechnician { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ActiveOutagesCount { get; set; }
    }
    // Used to CREATE a new crew (POST)
    public class CreateCrewDto
    {
        public string Name { get; set; } = string.Empty;
        public string LeadTechnician { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
    // Used to UPDATE a crew (PUT)
    public class UpdateCrewDto
    {
        public string Name { get; set; } = string.Empty;
        public string LeadTechnician { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public CrewStatus Status { get; set; }
    }
}
