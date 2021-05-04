using EventsAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [HttpGet("{id:int}/participants")] // GET /events/1/partipants
        public async Task<ActionResult> GetPartipantsForEvent(int id)
        {
            var data = await _context.Events
                .Where(e => e.Id == id)
                .Select(e => new { e.Id, e.Participants })
                .SingleOrDefaultAsync();

            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new { data = data.Participants } );
            }
        }


        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
        public async Task<ActionResult> AddEvent([FromBody] PostEventRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // Add it to the database
                // Return a 201 with a Location  Header (hard to do right now... but we will)
                var eventToAdd = new Event()
                {
                    Name = request.Name,
                    HostedBy = request.HostedBy,
                    LongDescription = request.LongDescription,
                    StartDateAndTime = request.StartDateAndTime.Value,
                    EndDateAndTime = request.EndDateAndTime.Value
                };
                _context.Events.Add(eventToAdd);
                await _context.SaveChangesAsync();
                //return Created()
                //var url = Url.Action(nameof(EventsController.GetById), nameof(EventsController), new { id = eventToAdd.Id });
                //return Created(url, eventToAdd); 
                return CreatedAtRoute("get-event-by-id", new { id = eventToAdd.Id }, eventToAdd); // this is a bit wrong.. stay with me.
            }

        }

        [HttpGet("{id:int}", Name = "get-event-by-id")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok();
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

    public record PostEventRequest(
        [Required]
        string Name,
        string LongDescription,
        [Required]
        string HostedBy,
        [Required]
        DateTime? StartDateAndTime,
        [Required]
        DateTime? EndDateAndTime
        );
}
