using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Data
{
    public class EventsDataContext : DbContext
    {

        public EventsDataContext(DbContextOptions<EventsDataContext> options): base(options)
        {

        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EventRegistration> EventRegistrations { get; set; }
    }
}
