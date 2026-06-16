namespace UtilityOMS.API.Models
{
    public class Outage
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public OutageStatus Status { get; set; } = OutageStatus.Reported;
        public OutageCause Cause { get; set; } = OutageCause.Unknown;
        public DateTime ReportedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // Location
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AffectedArea { get; set; } = string.Empty;
        public int AffectedCustomers { get; set; }

        // Who Reported  it
        public string Reportedby { get; set; }

        // Assigned Crew
        public int? AssignedCrewId { get; set; }
        public Crew? AssigendCrew { get; set; }

    }

    public enum OutageStatus
    {
        Reported,
        InProgress,
        Resolved
    }

    public enum OutageCause
    {
        Storm,
        Equipment,
        Accident,
        Maintenance,
        Unknown

    }
}
