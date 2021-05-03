using EventsAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Controllers
{
    [Route("events")]
    public class EventsController : ControllerBase
    {

        private readonly EventsDataContext _context;

        public EventsController(EventsDataContext context)
        {
            _context = context;
        }

        [HttpGet] // GET /events, GET /events?showPast=true
        public async Task<ActionResult> Get([FromQuery] bool showPast = false)
        {

            var details = await _context.Events
                .Where(e => e.EndDateAndTime.Date > DateTime.Now.Date)
                .Select(e => new GetEventsResponseItem(e.Id, e.Name, e.StartDateAndTime, e.EndDateAndTime, e.Participants.Count()))
                .ToListAsync();

            return Ok(new GetResponse<GetEventsResponseItem>(details));
        }
    }

    public record GetResponse<T>(IList<T> Data);

    public record GetEventsResponseItem(int Id, string Name, DateTime StartDate, DateTime EndDate, int NumberOfParticipants);
}
