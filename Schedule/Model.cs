using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Schedule
{
        public class Carrier
        {
            public int code { get; set; }
            public string title { get; set; }
            public Codes codes { get; set; }
        }

        public class Codes
        {
            public string sirena { get; set; }
            public string iata { get; set; }
            public string icao { get; set; }
        }

        public class Pagination
        {
            public int total { get; set; }
            public int limit { get; set; }
            public int offset { get; set; }
        }

        public class Root
        {
            public object date { get; set; }
            public Station station { get; set; }
            public string @event { get; set; }
            public Pagination pagination { get; set; }
            public List<Schedule> schedule { get; set; }
            public List<object> interval_schedule { get; set; }
        }

        public class Schedule
        {
            public Thread thread { get; set; }
            public string terminal { get; set; }
            public bool is_fuzzy { get; set; }
            public string stops { get; set; }
            public string platform { get; set; }
            public object except_days { get; set; }
            public string departure { get; set; }
            public object arrival { get; set; }
            public string days { get; set; }
        }

        public class Station
        {
            public string type { get; set; }
            public string title { get; set; }
            public string short_title { get; set; }
            public string popular_title { get; set; }
            public string code { get; set; }
            public string station_type { get; set; }
            public string station_type_name { get; set; }
            public string transport_type { get; set; }
        }

        public class Thread
        {
            public string number { get; set; }
            public string title { get; set; }
            public string short_title { get; set; }
            public Carrier carrier { get; set; }
            public string vehicle { get; set; }
            public string transport_type { get; set; }
            public object express_type { get; set; }
            public TransportSubtype transport_subtype { get; set; }
            public string uid { get; set; }
        }

        public class TransportSubtype
        {
            public object title { get; set; }
            public object code { get; set; }
            public object color { get; set; }
        }
    }

