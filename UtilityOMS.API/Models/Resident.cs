namespace UtilityOMS.API.Models
{
    public class Resident
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Location On Map
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        // Notification Preferences
        public bool SmsOptIn { get; set; } = true;
        public bool EmailOptIn { get; set; } = true;

        public DateTime RegisteredAt { get; set; } 



    }
}
