using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UtilityOMS.API.Data;
using UtilityOMS.API.DTOs;
using UtilityOMS.API.Hubs;
using UtilityOMS.API.Models;

namespace UtilityOMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<OutageHub> _hubContext;

        public OutageController(
            AppDbContext context,
            IHubContext<OutageHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // ✅ GET all outages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutageResponseDto>>> GetAll()
        {
            var outages = await _context.Outages
                .Include(o => o.AssigendCrew)
                .ToListAsync();

            return Ok(outages.Select(MapToDto));
        }

        // ✅ GET single outage by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OutageResponseDto>> GetById(int id)
        {
            var o = await _context.Outages
                .Include(o => o.AssigendCrew)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (o == null)
                return NotFound($"Outage with ID {id} not found.");

            return Ok(MapToDto(o));
        }

        // ✅ GET outages by status
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OutageResponseDto>>> GetByStatus(
            OutageStatus status)
        {
            var outages = await _context.Outages
                .Include(o => o.AssigendCrew)
                .Where(o => o.Status == status)
                .ToListAsync();

            return Ok(outages.Select(MapToDto));
        }

        // ✅ POST create new outage
        [HttpPost]
        public async Task<ActionResult<OutageResponseDto>> Create(CreateOutageDto dto)
        {
            var outage = new Outage
            {
                Title = dto.Title,
                Description = dto.Description,
                Cause = dto.Cause,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                AffectedArea = dto.AffectedArea,
                AffectedCustomers = dto.AffectedCustomers,
                Reportedby = dto.ReportedBy,
                Status = OutageStatus.Reported,
                ReportedAt = DateTime.UtcNow
            };

            _context.Outages.Add(outage);
            await _context.SaveChangesAsync();

            // ✅ Broadcast new outage to ALL map viewers in real-time!
            await _hubContext.Clients
                .Group("MapViewers")
                .SendAsync("OutageCreated", MapToDto(outage));

            return CreatedAtAction(
                nameof(GetById),
                new { id = outage.Id },
                MapToDto(outage));
        }

        // ✅ PUT update outage
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateOutageDto dto)
        {
            var outage = await _context.Outages
                .Include(o => o.AssigendCrew)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (outage == null)
                return NotFound($"Outage with ID {id} not found.");

            outage.Title = dto.Title;
            outage.Description = dto.Description;
            outage.Status = dto.Status;
            outage.Cause = dto.Cause;
            outage.AffectedArea = dto.AffectedArea;
            outage.AffectedCustomers = dto.AffectedCustomers;
            outage.AssignedCrewId = dto.AssignedCrewId;

            if (dto.Status == OutageStatus.Resolved && outage.ResolvedAt == null)
                outage.ResolvedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // ✅ Broadcast updated outage to ALL map viewers!
            await _hubContext.Clients
                .Group("MapViewers")
                .SendAsync("OutageUpdated", MapToDto(outage));

            return NoContent();
        }

        // ✅ DELETE outage
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var outage = await _context.Outages.FindAsync(id);

            if (outage == null)
                return NotFound($"Outage with ID {id} not found.");

            _context.Outages.Remove(outage);
            await _context.SaveChangesAsync();

            // ✅ Broadcast deletion to ALL map viewers!
            await _hubContext.Clients
                .Group("MapViewers")
                .SendAsync("OutageDeleted", id);

            return NoContent();
        }

        // ✅ Helper - Map entity to DTO
        private static OutageResponseDto MapToDto(Outage o) => new()
        {
            Id = o.Id,
            Title = o.Title,
            Description = o.Description,
            Status = o.Status.ToString(),
            Cause = o.Cause.ToString(),
            ReportedAt = o.ReportedAt,
            ResolvedAt = o.ResolvedAt,
            Latitude = o.Latitude,
            Longitude = o.Longitude,
            AffectedArea = o.AffectedArea,
            AffectedCustomers = o.AffectedCustomers,
            ReportedBy = o.Reportedby,
            AssignedCrewId = o.AssignedCrewId,
            AssignedCrewName = o.AssigendCrew?.Name
        };
    }
}