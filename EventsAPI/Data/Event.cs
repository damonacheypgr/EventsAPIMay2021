using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Data
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LongDescription { get;  set; }
        public string HostedBy { get; set; }
        public DateTime StartDateAndTime { get; set; }
        public DateTime EndDateAndTime { get; set; }
        public IList<EventParticipant> Participants { get; set; }
    }

    public class EventParticipant
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
    }

}
