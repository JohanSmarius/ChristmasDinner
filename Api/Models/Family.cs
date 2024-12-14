using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Family
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public int NumberOfPersons { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfSeats { get; set; }
        public List<Guest> Guests { get; set; } = new List<Guest>();
    }

    public class Guest
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
