
using hotellisting.api.Contracts;
using hotellisting.api.data;
using hotellisting.api.DTOs.Country;
using hotellisting.api.DTOs.Hotel;
using hotellisting.api.Results;
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
            var result = await countriesService.GetCountriesAsync();
            return ToActionResult(result);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
        {
            var result = await countriesService.GetCountryAsync(id);
            return ToActionResult(result);
        }

        // PUT: api/Countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateDto)
        {
            var result = await countriesService.UpdateCountryAsync(id, updateDto);
            return ToActionResult(result);
        }

        // POST: api/Countries
        [HttpPost]
        public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto createDto)
        {
            var result = await countriesService.CreateCountryAsync(createDto);
            if (!result.IsSuccess) return MapErrorsToResponse(result.Errors);

            return CreatedAtAction(nameof(GetCountry), new { id = result.Value!.Id }, result.Value);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var result = await countriesService.DeleteCountryAsync(id);
            return ToActionResult(result);
        }

        private ActionResult<T> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result.Value) : MapErrorsToResponse(result.Errors);

        private ActionResult ToActionResult(Result result)
            => result.IsSuccess ? NoContent() : MapErrorsToResponse(result.Errors);

        private ActionResult MapErrorsToResponse(Error[] errors)
        {
            if (errors is null || errors.Length == 0) return Problem();

            var e = errors[0];
            return e.Code switch
            {
                "NotFound" => NotFound(e.Description),
                "BadRequest" => BadRequest(e.Description),
                "Validation" => BadRequest(e.Description),
                "Conflict" => Conflict(e.Description),
                _ => Problem(detail: string.Join("; ", errors.Select(x => x.Description)), title: e.Code)
            };
        }
    }
}
