
using hotellisting.api.Contracts;
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
    public class CountriesController(ICountriesService countriesService) : ControllerBase
    {
  




        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
        {
            var countries = await countriesService.GetCountriesAsync();
            return Ok(countries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
        {
            var country = await countriesService.GetCountryAsync(id);

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

            await countriesService.UpdateCountryAsync(id, updateDto);
            return NoContent();
        }

        // POST: api/Countries

        [HttpPost]
        public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto createDto)
        {
            // var country = new Country
            // {
            //     Name = createDto.Name,
            //     ShortName = createDto.ShortName
            // };

            // _context.Countries.Add(country);
            // await _context.SaveChangesAsync();

            // var resultDto = new GetCountryDto(
            //     country.CountryId,
            //     country.Name,
            //     country.ShortName,
            //     []
            // );
            
            var resultDto = await countriesService.CreateCountryAsync(createDto);
            return CreatedAtAction(nameof(GetCountry), new { id = resultDto.Id }, resultDto);
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
     await countriesService.DeleteCountryAsync(id);

        return NoContent();
    }


    }
}
