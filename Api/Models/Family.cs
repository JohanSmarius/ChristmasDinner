namespace Api.Models
{
    public class Family
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public int NumberOfPersons { get; set; }
        public List<Child> Children { get; set; }
        public int NumberOfSeats { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
