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
            var customerCityEvents = marketingEngine.GetCustomerCityEvents(customer);
            var customerBirthDayEvents = marketingEngine.GetCustomerBirthDayEvents(customer, 7);
            var customerClosestEvents = marketingEngine.GetCustomerClosestEvents(customer, 50, 5);
            var customerEvents = customerCityEvents.Union(customerBirthDayEvents).Union(customerClosestEvents).ToList();
            customerEvents.ForEach(e => MarketingEngine.SendCustomerNotifications(customer, e));
        }

        public record City(string Name, int X, int Y);
        public static readonly IDictionary<string, City> Cities = new Dictionary<string, City>()
        {
            { "New York", new City("New York", 3572, 1455) },
            { "Los Angeles", new City("Los Angeles", 462, 975) },
            { "San Francisco", new City("San Francisco", 183, 1233) },
            { "Boston", new City("Boston", 3778, 1566) },
            { "Chicago", new City("Chicago", 2608, 1525) },
            { "Washington", new City("Washington", 3358, 1320) },
        };

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

            public List<Event> GetCustomerBirthDayEvents(Customer customer, int daysBefore)
            {
                var nextBirthDate = GetCustomerNextBirthDay(customer);

                return _events.Where(e => (nextBirthDate - e.Date).Days == daysBefore).ToList();
            }

            public List<Event> GetCustomerClosestEvents(Customer customer, int maxDistance, int eventCount)
            {
                var customerCity = Cities.First(c => c.Key == customer.City).Value;
                return _events
                    .Select(e => new { Event = e, Distance = GetDistance(customerCity, e) })
                    .Where(e => e.Distance.HasValue && e.Distance <= maxDistance)
                    .OrderBy(e => e.Distance)
                    .Take(eventCount)
                    .Select(e => e.Event)
                    .ToList();
            }

            public static void SendCustomerNotifications(Customer customer, Event e)
            {
                Console.WriteLine($"{customer.Name} from {customer.City} event {e.Name} at {e.Date}");
            }

            private static DateTime GetCustomerNextBirthDay(Customer customer)
            {
                var nextBirthDayYear = DateTime.Now.Year;
                var thisYearBirthDay = new DateTime(nextBirthDayYear, customer.BirthDate.Month, customer.BirthDate.Day);
                if (thisYearBirthDay < DateTime.Now)
                    nextBirthDayYear++;

                return new DateTime(nextBirthDayYear, customer.BirthDate.Month, customer.BirthDate.Day);
            }

            private static int? GetDistance(City city, Event e)
            {
                if (!Cities.ContainsKey(e.City))
                    return null;

                return GetDistance(city, Cities.First(c => c.Key == e.City).Value);
            }

            private static int GetDistance(City city1, City city2)
            {
                return Math.Abs(city1.X - city2.X) + Math.Abs(city1.Y - city2.Y);
            }
        }
    }
}