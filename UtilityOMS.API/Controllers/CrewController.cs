using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityOMS.API.Data;
using UtilityOMS.API.DTOs;
using UtilityOMS.API.Models;

namespace UtilityOMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CrewController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET all crews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrewResponseDto>>> GetAll()
        {
            var crews = await _context.Crews
                .Include(c => c.Outages)
                .ToListAsync();

            var result = crews.Select(c => new CrewResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                LeadTechnician = c.LeadTechnician,
                PhoneNumber = c.PhoneNumber,
                Status = c.Status.ToString(),
                ActiveOutagesCount = c.Outages.Count(o => o.Status != OutageStatus.Resolved)
            });

            return Ok(result);
        }

        // ✅ GET single crew by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CrewResponseDto>> GetById(int id)
        {
            var c = await _context.Crews
                .Include(c => c.Outages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (c == null)
                return NotFound($"Crew with ID {id} not found.");

            return Ok(new CrewResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                LeadTechnician = c.LeadTechnician,
                PhoneNumber = c.PhoneNumber,
                Status = c.Status.ToString(),
                ActiveOutagesCount = c.Outages.Count(o => o.Status != OutageStatus.Resolved)
            });
        }

        // ✅ POST create new crew
        [HttpPost]
        public async Task<ActionResult<CrewResponseDto>> Create(CreateCrewDto dto)
        {
            var crew = new Crew
            {
                Name = dto.Name,
                LeadTechnician = dto.LeadTechnician,
                PhoneNumber = dto.PhoneNumber,
                Status = CrewStatus.Available
            };

            _context.Crews.Add(crew);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = crew.Id }, new CrewResponseDto
            {
                Id = crew.Id,
                Name = crew.Name,
                LeadTechnician = crew.LeadTechnician,
                PhoneNumber = crew.PhoneNumber,
                Status = crew.Status.ToString()
            });
        }

        // ✅ PUT update crew
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCrewDto dto)
        {
            var crew = await _context.Crews.FindAsync(id);

            if (crew == null)
                return NotFound($"Crew with ID {id} not found.");

            crew.Name = dto.Name;
            crew.LeadTechnician = dto.LeadTechnician;
            crew.PhoneNumber = dto.PhoneNumber;
            crew.Status = dto.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE crew
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var crew = await _context.Crews.FindAsync(id);

            if (crew == null)
                return NotFound($"Crew with ID {id} not found.");

            _context.Crews.Remove(crew);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
