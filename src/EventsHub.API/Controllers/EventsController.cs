using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsHub.Domain;
using EventsHub.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsHub.API.Controllers;

public class EventsController(AppDbContext context) : EventsHubBaseController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Event>>> GetEventsAsync()
    {
        return await context.Events.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEventAsync(string id)
    {
        var result = await context.Events.FindAsync(id);
        if (result is null)
        {
            return NotFound("The event with the specified ID was not found.");
        }
        return result;
    }
}