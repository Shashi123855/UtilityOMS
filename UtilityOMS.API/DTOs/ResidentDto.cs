namespace UtilityOMS.API.DTOs
{
    // Used to READ resident data (GET)
    public class ResidentResponseDto
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
    // Used to CREATE a new resident (POST)
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
    // Used to UPDATE a resident (PUT)
    public class UpdateResidentDto
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool SmsOptIn { get; set; }
        public bool EmailOptIn { get; set; }
    }
}
