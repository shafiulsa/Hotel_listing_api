using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hotellisting.api.data;
using hotellisting.api.DTOs.Country;
using hotellisting.api.DTOs.Hotel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotellisting.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _context;

        public CountriesController(HotelListingDbContext context)
        {
            _context = context;
        }





        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
        {
            var countries = await _context.Countries
                .Select(c => new GetCountriesDto(
                    c.CountryId,
                    c.Name,
                    c.ShortName
                ))
                .ToListAsync();

            return Ok(countries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
        {
            var country = await _context.Countries
                .Where(q => q.CountryId == id)
                .Select(c => new GetCountryDto(
                    c.CountryId,
                    c.Name,
                    c.ShortName,
                    c.Hotels.Select(h => new GetHotelSlimDto(
                        h.Id,
                        h.Name,
                        h.Address,
                        h.Rating
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        // PUT: api/Countries/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateDto)
        {   
            if (id != updateDto.Id)
            {
                return BadRequest();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            country.Name = updateDto.Name;
            country.ShortName = updateDto.ShortName;

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries

        [HttpPost]
        public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto createDto)
        {
            var country = new Country
            {
                Name = createDto.Name,
                ShortName = createDto.ShortName
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            var resultDto = new GetCountryDto(
                country.CountryId,
                country.Name,
                country.ShortName,
                []
            );

            return CreatedAtAction(nameof(GetCountry), new { id = country.CountryId }, resultDto);
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound();
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CountryExistsAsync(int id)
    {
        return await _context.Countries.AnyAsync(e => e.CountryId == id);
    }
    }
}
