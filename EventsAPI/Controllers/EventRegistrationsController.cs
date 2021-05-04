using EventsAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Controllers
{
    public class EventRegistrationsController : ControllerBase
    {
        public readonly EventsDataContext _context;
        public readonly EventRegistrationChannel _channel;

        public EventRegistrationsController(EventsDataContext context, EventRegistrationChannel channel)
        {
            _context = context;
            _channel = channel;
        }

        [HttpPost("events/{eventId;int}/registrations")]
        public async Task<ActionResult> AddRegistration(int eventId, [FromBody] PostParticipantsRequest request)
        {
            var savedEvent = await _context.Events.SingleOrDefaultAsync(e => e.Id == eventId);

            if (savedEvent == null)
                return NotFound();

            var registration = new EventRegistration
            {
                EmployeeId = request.Id,
                Name = request.FirstName + " " + request.LastName,
                EMail = request.Email,
                Phone = request.Phone,
                Status = EventRegistrationStatus.Pending,
            };

            savedEvent.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            var worked = await _channel.AddRegistration(new EventRegistrationChannelRequest(registration.Id));

            if (!worked)
            {
                // TODO:
            }

            return CreatedAtRoute(
                "get-event-reservation",
                new
                {
                    eventId = savedEvent.Id,
                    registrationId = registration.Id
                },
                registration);
        }

        [HttpGet("events/{eventId;int}/registrations/{registrationId:int}", Name = "get-event-reservation")]
        public async Task<ActionResult> LookupRegistration(int eventId, int registrationId)
        {
            var response = await _context.Events
                .Where(e => e.Id == eventId)
                .Select(e => e.Registrations.Where(r => r.Id == registrationId))
                .SingleOrDefaultAsync();

            if (response == null)
                return NotFound();
            else
                return Ok(response.First());
        }

        [HttpGet("events/{eventId:int}/registrations")]
        public async Task<ActionResult> GetRegistrationsForEvent(int eventId)
        {
            return Ok();
        }
    }

    public record PostReservationRequest
    {
        public int Id { get; init; }

        public string FirstName { get; init; }
        public string LastName { get; init; }

        public string Email { get; init; }
        public string Phone { get; init; }
    }

    public record GetReservationResponse
    {
        public int Id { get; init; }
        public int EmployeeId { get; set; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public string Email { get; init; }
        public string Phone { get; init; }
        public EventRegistration Status { get; set; }
        public string DeniedReason { get; set; }
    }
}
