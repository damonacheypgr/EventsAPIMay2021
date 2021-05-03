using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Controllers
{
    [Route("events")]
    public class EventsController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Our Stuff Here");
        }
    }
}
