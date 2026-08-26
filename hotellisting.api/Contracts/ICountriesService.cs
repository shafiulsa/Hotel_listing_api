using hotellisting.api.DTOs.Country;
using hotellisting.api.Results;

namespace hotellisting.api.Contracts;

public interface ICountriesService
{
    // Task<IEnumerable<GetCountriesDto>> GetCountriesAsync();
    // Task<GetCountryDto?> GetCountryAsync(int id);
    // Task<GetCountryDto> CreateCountryAsync(CreateCountryDto createDto);
    // Task UpdateCountryAsync(int id, UpdateCountryDto updateDto);
    // Task DeleteCountryAsync(int id);

    // Task<bool> CountryExistsAsync(int id);
    // Task<bool> CountryExistsAsync(string name); 
    Task<bool> CountryExistsAsync(int id);
    Task<bool> CountryExistsAsync(string name);
    Task<Result<GetCountryDto>> CreateCountryAsync(CreateCountryDto createDto);
    Task<Result> DeleteCountryAsync(int id);
    Task<Result<IEnumerable<GetCountriesDto>>> GetCountriesAsync();
    Task<Result<GetCountryDto>> GetCountryAsync(int id);
    Task<Result> UpdateCountryAsync(int id, UpdateCountryDto updateDto);
}
