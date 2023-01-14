using static StubhubAssessment.Program;

namespace StubhubAssessment
{
    public class Program
    {
        static void Main(string[] args)
        {
            var events = new List<Event>
            {
                new Event(1, "Phantom of the Opera", "New York", new DateTime(2023, 12, 23)),
                new Event(2, "Metallica", "Los Angeles", new DateTime(2023, 12, 02)),
                new Event(3, "Metallica", "New York", new DateTime(2023, 12, 06)),
                new Event(4, "Metallica", "Boston", new DateTime(2023, 10, 23)),
                new Event(5, "LadyGaGa", "New York", new DateTime(2023, 09, 20)),
                new Event(6, "LadyGaGa", "Boston", new DateTime(2023, 08, 01)),
                new Event(7, "LadyGaGa", "Chicago", new DateTime(2023, 07, 04)),
                new Event(8, "LadyGaGa", "San Francisco", new DateTime(2023, 07, 07)),
                new Event(9, "LadyGaGa", "Washington", new DateTime(2023, 05, 22)),
                new Event(10, "Metallica", "Chicago", new DateTime(2023, 01, 01)),
                new Event(11, "Phantom of the Opera", "San Francisco", new DateTime(2023, 07, 04)),
                new Event(12, "Phantom of the Opera", "Chicago", new DateTime(2024, 05, 15))
            };
            var customer = new Customer()
            {
                Id = 1,

                Name = "John",
                City = "New York",
                BirthDate = new DateTime(1995, 05, 10)
            };

            MarketingEngine marketingEngine = new(events);
            var customerEvents = marketingEngine.GetCustomerCityEvents(customer);
            customerEvents.ForEach(e => MarketingEngine.SendCustomerNotifications(customer, e));
        }
        public class Event
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public DateTime Date { get; set; }
            public Event(int id, string name, string city, DateTime date)
            {
                this.Id = id;
                this.Name = name;
                this.City = city;
                this.Date = date;
            }
        }
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public DateTime BirthDate { get; set; }
        }
    }

    public class MarketingEngine
    {
        private readonly List<Event> _events;
        public MarketingEngine(List<Event> events)
        {
            _events = events;
        }

        public List<Event> GetCustomerCityEvents(Customer customer)
        {
            return _events.Where(e => e.City == customer.City).ToList();
        }
        public static void SendCustomerNotifications(Customer customer, Event e)
        {
            Console.WriteLine($"{customer.Name} from {customer.City} event {e.Name} at {e.Date}");
        }
    }
}