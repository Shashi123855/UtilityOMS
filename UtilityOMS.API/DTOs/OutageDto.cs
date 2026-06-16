using UtilityOMS.API.Models;

namespace UtilityOMS.API.DTOs
{
    // Used to READ outage data (GET)
    public class OutageResponseDto
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

    // Used to CREATE a new outage (POST)
    public class CreateOutageDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public OutageCause Cause { get; set; } = OutageCause.Unknown;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AffectedArea { get; set; } = string.Empty;
        public int AffectedCustomers { get; set; }
        public string ReportedBy { get; set; } = string.Empty;
    }
    // Used to UPDATE an outage (PUT)
    public class UpdateOutageDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public OutageStatus Status { get; set; }
        public OutageCause Cause { get; set; }
        public string AffectedArea { get; set; } = string.Empty;
        public int AffectedCustomers { get; set; }
        public int? AssignedCrewId { get; set; }
    }
}
