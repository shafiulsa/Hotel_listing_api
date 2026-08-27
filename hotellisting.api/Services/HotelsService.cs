using AutoMapper;
using AutoMapper.QueryableExtensions;
using hotellisting.api.Constants;
using hotellisting.api.Contracts;
using hotellisting.api.data;
using hotellisting.api.DTOs.Hotel;
using hotellisting.api.Results;
using Microsoft.EntityFrameworkCore;

namespace hotellisting.api.Services;

public class HotelsService(HotelListingDbContext context,
ICountriesService countriesService,
IMapper mapper) : IHotelsService
{
    // public async Task<IEnumerable<GetHotelsDto>> GetHotelsAsync()
    // {
    //     return await context.Hotels
    //      .Include(q=>q.Country)
    //         .Select(h => new GetHotelsDto(
    //             h.Id,
    //             h.Name,
    //             h.Address,
    //             h.Rating,
    //             h.CountryId
    //         ))
    //         .ToListAsync();
    // }

    // public async Task<GetHotelDto?> GetHotelAsync(int id)
    // {
    //     var hotel = await context.Hotels
    //         .Where(q => q.Id == id)
    //         // .Select(c => new GetHotelDto(
    //         //     c.Id,
    //         //     c.Name,
    //         //     c.Address,
    //         //     c.Rating,
    //         //     c.Country!.Name
    //         // ))
    //         .Include(q => q.Country)
    //         .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
    //         .FirstOrDefaultAsync();

    //     return hotel;
    // }

    // public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto createDto)
    // {
    //     // var hotel = new Hotel
    //     // {
    //     //     Name = createDto.Name,
    //     //     Address = createDto.Address,
    //     //     Rating = createDto.Rating,
    //     //     CountryId = createDto.CountryId
    //     // };
    //     var hotel = mapper.Map<Hotel>(createDto);

    //     context.Hotels.Add(hotel);
    //     await context.SaveChangesAsync();

    //     var countryName = await context.Countries
    //         .Where(c => c.CountryId == createDto.CountryId)
    //         .Select(c => c.Name)
    //         .FirstOrDefaultAsync() ?? string.Empty;

    //     // return new GetHotelDto(
    //     //     hotel.Id,
    //     //     hotel.Name,
    //     //     hotel.Address,
    //     //     hotel.Rating,
    //     //     countryName 
    //     // );
    //     return mapper.Map<GetHotelDto>(hotel);
    // }

    // public async Task UpdateHotelAsync(int id, UpdateHotelDto updateDto)
    // {
    //     var hotel = await context.Hotels.FindAsync(id)
    //         ?? throw new KeyNotFoundException("Hotel not found");

    //     hotel.Name = updateDto.Name;
    //     hotel.Address = updateDto.Address;
    //     hotel.Rating = updateDto.Rating;
    //     hotel.CountryId = updateDto.CountryId;

    //     await context.SaveChangesAsync();
    // }

    // public async Task DeleteHotelAsync(int id)
    // {
    //     var hotel = await context.Hotels.FindAsync(id)
    //         ?? throw new KeyNotFoundException("Hotel not found");

    //     context.Hotels.Remove(hotel);
    //     await context.SaveChangesAsync();
    // }
    public async Task<Result<IEnumerable<GetHotelDto>>> GetHotelsAsync()
    {
        var hotels = await context.Hotels
            .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetHotelDto>>.Success(hotels);
    }

    public async Task<Result<GetHotelDto>> GetHotelAsync(int id)
    {
        var hotel = await context.Hotels
            .Where(h => h.Id == id)
            .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (hotel is null)
        {
            return Result<GetHotelDto>.Failure(new Error(ErrorCodes.NotFound, $"Hotel '{id}' was not found."));
        }

        return Result<GetHotelDto>.Success(hotel);
    }

    public async Task<Result<GetHotelDto>> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        var countryExists = await countriesService.CountryExistsAsync(hotelDto.CountryId);
        if (!countryExists)
        {
            return Result<GetHotelDto>.Failure(new Error(ErrorCodes.NotFound, $"Country '{hotelDto.CountryId}' was not found."));
        }

        var duplicate = await HotelExistsAsync(hotelDto.Name, hotelDto.CountryId);
        if (duplicate)
        {
            return Result<GetHotelDto>.Failure(new Error(ErrorCodes.Conflict, $"Hotel '{hotelDto.Name}' already exists in the selected country."));
        }

        var hotel = mapper.Map<Hotel>(hotelDto);
        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();

        var dto = await context.Hotels
            .Where(h => h.Id == hotel.Id)
            .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
            .FirstAsync();

        return Result<GetHotelDto>.Success(dto);
    }

    public async Task<Result> UpdateHotelAsync(int id, UpdateHotelDto updateDto)
    {
        if (id != updateDto.Id)
        {
            return Result.BadRequest(new Error(ErrorCodes.Validation, "Id route value does not match payload Id."));
        }

        var hotel = await context.Hotels.FindAsync(id);
        if (hotel is null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Hotel '{id}' was not found."));
        }

        var countryExists = await countriesService.CountryExistsAsync(updateDto.CountryId);
        if (!countryExists)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Country '{updateDto.CountryId}' was not found."));
        }

        var duplicate = await HotelExistsAsync(updateDto.Name, updateDto.CountryId);
        if (duplicate)
        {
            return Result.Failure(new Error(ErrorCodes.Conflict, $"Hotel '{updateDto.Name}' already exists in the selected country."));
        }

        hotel.Name = updateDto.Name;
        hotel.Address = updateDto.Address;
        hotel.Rating = updateDto.Rating;
        hotel.CountryId = updateDto.CountryId;

        context.Hotels.Update(hotel);
        await context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteHotelAsync(int id)
    {
        var affected = await context.Hotels
            .Where(q => q.Id == id)
            .ExecuteDeleteAsync();

        if (affected == 0)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Hotel '{id}' was not found."));
        }

        return Result.Success();
    }

    public async Task<bool> HotelExistsAsync(int id)
    {
        return await context.Hotels.AnyAsync(e => e.Id == id);
    }

    public async Task<bool> HotelExistsAsync(string name, int countryId)
    {
        return await context.Hotels.AnyAsync(h => h.Name == name && h.CountryId == countryId);
    }
}
