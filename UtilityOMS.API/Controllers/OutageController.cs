using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityOMS.API.Data;
using UtilityOMS.API.DTOs;
using UtilityOMS.API.Models;

namespace UtilityOMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutageController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OutageController(AppDbContext context)
        {
            _context = context;
        }

        // Get All Outages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutageResponseDto>>> GetAll()
        {
            var outages = await _context.Outages.Include(o => o.AssigendCrew).ToListAsync();
            var result = outages.Select(o => new OutageResponseDto
            {
                Id= o.Id,
                Title = o.Title,
                Description = o.Description,
                Status = o.Status.ToString(),
                Cause = o.Cause.ToString(),
                ReportedAt = o.ReportedAt,
                ResolvedAt = o.ResolvedAt,
                Latitude    = o.Latitude,
                Longitude   = o.Longitude,
                AffectedArea = o.AffectedArea,
                AffectedCustomers = o.AffectedCustomers,
                ReportedBy = o.Reportedby,
                AssignedCrewId = o.AssignedCrewId,
                AssignedCrewName = o.AssigendCrew?.Name
            });
            return Ok(result);
        }

        // Get Single Outage By Id 
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OutageResponseDto>>> GetById(int id)
        {
            var o = await _context.Outages.Include(o => o.AssigendCrew).FirstOrDefaultAsync(o => o.Id == id);
            if (o == null)
                return NotFound($"Outage with ID {id} not found");
            return Ok(new OutageResponseDto
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
            });
        }

        // Get Outages By Status 
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OutageResponseDto>>>GetByStatus(OutageStatus status)
        {
            var outages = await _context.Outages.Include(o => o.AssigendCrew).Where(o => o.Status == status).ToListAsync();
            var result = outages.Select(o => new OutageResponseDto
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
            });
            return Ok(result);
        }

        // Create an Outage
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
            return CreatedAtAction(nameof(GetById), new { id = outage.Id }, new OutageResponseDto
            {
                Id = outage.Id,
                Title = outage.Title,
                Description = outage.Description,
                Status = outage.Status.ToString(),
                Cause = outage.Cause.ToString(),
                ReportedAt = outage.ReportedAt,
                Latitude = outage.Latitude,
                Longitude = outage.Longitude,
                AffectedArea = outage.AffectedArea,
                AffectedCustomers = outage.AffectedCustomers,
                ReportedBy = outage.Reportedby
            });
        }

        // Update an Outage by id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateOutageDto dto)
        {
            var outage = await _context.Outages.FindAsync(id);
            if (outage == null)
                return NotFound($"Outage with the ID {id} not found");
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
            return NoContent();
        }

        // Delete Outage
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var outage = await _context.Outages.FindAsync(id);
            if (outage == null)
                return NotFound($"Outage with ID {id} not found");
            _context.Outages.Remove(outage);
           await _context.SaveChangesAsync();
            return NoContent();

        }


    }
}
