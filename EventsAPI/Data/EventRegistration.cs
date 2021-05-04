using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Data
{
    public enum EventRegistrationStatus {  Pending, Approved, Denied };

    public class EventRegistration
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }

        public EventRegistrationStatus Status { get; set; }
        public string ReasonFoDenial { get; set; }
    }
}
