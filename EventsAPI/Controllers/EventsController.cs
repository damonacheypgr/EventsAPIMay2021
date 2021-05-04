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
        private readonly IEmployeeService _employeeService;

        public EventsController(EventsDataContext context, IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        [HttpHead("{id:int}")]
        public async Task<ActionResult> CheckForEmployee(int id)
        {
            var any = await _context.Employees.Where(e => e.Id == id && e.IsActive).AnyAsync();

            return any ? Ok() : NotFound();
        }

        [HttpHead("emails/{emailAddress}")]
        public async Task<ActionResult> CheckForEmailAddress(string emailAddress)
        {
            var any = await _context.Employees.Where(e => e.IsActive && e.EMail == emailAddress).AnyAsync();

            return any ? Ok() : NotFound();
        }

        [HttpGet("{id:int}/participants/{employeeId:int}")]
        public async Task<ActionResult> GetParticipantForEvent(int id, int employeeId)
        {
            return Redirect("http://localhost:1337/employee/" + employeeId);
        }

        [HttpPost("{id:int}/participants")]
        public async Task<ActionResult> AddParticipant(int id, [FromBody] PostParticipantsRequest request)
        {
            var savedEvent = await _context.Events.SingleOrDefaultAsync(e => e.Id == id);
            if (savedEvent == null)
            {
                return NotFound("No event with that id");
            }

            bool employeeIsActive = await _employeeService.IsActiveAsync(id);

            //EventParticipant participant = new();
            var participant = new EventParticipant
            {
                EmployeeId = request.Id,
                Name = request.FirstName + " " + request.LastName,
                EMail = request.Email,
                Phone = request.Phone,
            };

            savedEvent.Participants ??= new List<EventParticipant>();

            savedEvent.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id:int}/participants")] // GET /events/1/partipants
        public async Task<ActionResult> GetPartipantsForEvent(int id)
        {
            var data = await _context.Events
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    Participants = e.Participants
                        .Select(p => new GetParticipantResponse(p.Id, p.Name, p.EMail, p.Phone))
                })
                .SingleOrDefaultAsync();

            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new { data = data.Participants });
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

    public record PostParticipantsRequest
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public string Department { get; set; }
        //public int Salary { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
