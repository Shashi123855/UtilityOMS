namespace UtilityOMS.API.Models
{
    public class Crew
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LeadTechnician { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public CrewStatus Status { get; set; } = CrewStatus.Available;

        public List<Outage> Outages { get; set; } = new ();
    }
    public enum CrewStatus
    {
        Available,
        Dispatched,
        OnSite,
        OffDuty
    }
}
