using hotellisting.api.Contracts;
using hotellisting.api.data;
using hotellisting.api.DTOs.Country;
using hotellisting.api.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace hotellisting.api.Services;

public class CountriesService(HotelListingDbContext context) : ICountriesService
{
    public async Task<IEnumerable<GetCountriesDto>> GetCountriesAsync()
    {
        return await context.Countries
            .Select(c => new GetCountriesDto(
                c.CountryId,
                c.Name,
                c.ShortName
            ))
            .ToListAsync();
    }

    public async Task<GetCountryDto?> GetCountryAsync(int id)
    {
        var country = await context.Countries
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

        return country;
    }

    public async Task<GetCountryDto> CreateCountryAsync(CreateCountryDto createDto)
    {
        // var country = new Country
        // {
        //     Name = createDto.Name,
        //     ShortName = createDto.ShortName
        // };

        // context.Countries.Add(country);
        // await context.SaveChangesAsync();

        // return new GetCountryDto(
        //     country.CountryId,
        //     country.Name,
        //     country.ShortName,
        //     []
        // );
        var country = new Country
        {
            Name = createDto.Name,
            ShortName = createDto.ShortName
        };

        context.Countries.Add(country);
        await context.SaveChangesAsync();

        return new GetCountryDto(
            country.CountryId,
            country.Name,
            country.ShortName,
            []
        );
    }

    public async Task UpdateCountryAsync(int id, UpdateCountryDto updateDto)
    {
        var country = await context.Countries.FindAsync(id)
            ?? throw new KeyNotFoundException("Country not found");

        country.Name = updateDto.Name;
        country.ShortName = updateDto.ShortName;

        // EF Core change tracker automatically detects modifications
        await context.SaveChangesAsync();
    }

    public async Task DeleteCountryAsync(int id)
    {
        var country = await context.Countries.FindAsync(id)
            ?? throw new KeyNotFoundException("Country not found");

        context.Countries.Remove(country);
        await context.SaveChangesAsync();
    }

    public async Task<bool> CountryExistsAsync(int id)
    {
        return await context.Countries.AnyAsync(e => e.CountryId == id);
    }


    public async Task<bool> CountryExistsAsync(string name)
    {
        return await context.Countries.AnyAsync(e => e.Name == name);
    }
}
