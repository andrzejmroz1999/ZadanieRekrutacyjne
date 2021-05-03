using System;
using System.Collections.Generic;
using System.Text;

namespace ZadanieRekrutacyjne
{
    class Calendar
    {
        public class working_hours
        {
            public DateTime start = new DateTime();
            public DateTime end = new DateTime();
        }
        public class planned_meeting
        {
            public List<DateTime> start = new List<DateTime>();
            public List<DateTime> end = new List<DateTime>();
        }
    }
}
