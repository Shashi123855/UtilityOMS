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
    public class ResidentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ResidentController(AppDbContext context)
        {
            _context = context;
        }

        // Get All Residents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResidentResponseDto>>> GetAll()
        {
            var residents = await _context.Residents.ToListAsync();
            var result = residents.Select(r => new ResidentResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                PhoneNumber = r.PhoneNumber,
                Email = r.Email,
                Address = r.Address,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                SmsOptIn = r.SmsOptIn,
                EmailOptIn = r.EmailOptIn,
                RegisteredAt = r.RegisteredAt
            });
            return Ok(result);
        }

        // Get single Resident By id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ResidentResponseDto>>> GetById(int id)
        {
            var r = await _context.Residents.FindAsync(id);
            if (r == null)
                return NotFound($"Resident with this ID {id} not found ");
            return Ok(new ResidentResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                PhoneNumber = r.PhoneNumber,
                Email = r.Email,
                Address = r.Address,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                SmsOptIn = r.SmsOptIn,
                EmailOptIn = r.EmailOptIn,
                RegisteredAt = r.RegisteredAt

            });
        }

        // Create a new Resident 

        [HttpPost]
        public async Task<ActionResult<ResidentResponseDto>> Create(CreateResidentDto dto)
        {
            var resident = new Resident
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                SmsOptIn = dto.SmsOptIn,
                EmailOptIn = dto.EmailOptIn,
                RegisteredAt = DateTime.UtcNow
            };
            _context.Residents.Add(resident);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = resident.Id }, new ResidentResponseDto
            {
                Id = resident.Id,
                Name = resident.Name,
                PhoneNumber = resident.PhoneNumber,
                Email = resident.Email,
                Address = resident.Address,
                Latitude = resident.Latitude,
                Longitude = resident.Longitude,
                SmsOptIn = resident.SmsOptIn,
                EmailOptIn = resident.EmailOptIn,
                RegisteredAt = resident.RegisteredAt
            });
        }

        // ✅ PUT update resident
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateResidentDto dto)
        {
            var resident = await _context.Residents.FindAsync(id);

            if (resident == null)
                return NotFound($"Resident with ID {id} not found.");

            resident.Name = dto.Name;
            resident.PhoneNumber = dto.PhoneNumber;
            resident.Email = dto.Email;
            resident.Address = dto.Address;
            resident.SmsOptIn = dto.SmsOptIn;
            resident.EmailOptIn = dto.EmailOptIn;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE resident
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resident = await _context.Residents.FindAsync(id);

            if (resident == null)
                return NotFound($"Resident with ID {id} not found.");

            _context.Residents.Remove(resident);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
