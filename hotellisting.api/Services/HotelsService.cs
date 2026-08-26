using AutoMapper;
using AutoMapper.QueryableExtensions;
using hotellisting.api.Contracts;
using hotellisting.api.data;
using hotellisting.api.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace hotellisting.api.Services;

public class HotelsService(HotelListingDbContext context,IMapper mapper) : IHotelsService
{
    public async Task<IEnumerable<GetHotelsDto>> GetHotelsAsync()
    {
        return await context.Hotels
         .Include(q=>q.Country)
            .Select(h => new GetHotelsDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId
            ))
            .ToListAsync();
    }

    public async Task<GetHotelDto?> GetHotelAsync(int id)
    {
        var hotel = await context.Hotels
            .Where(q => q.Id == id)
            // .Select(c => new GetHotelDto(
            //     c.Id,
            //     c.Name,
            //     c.Address,
            //     c.Rating,
            //     c.Country!.Name
            // ))
            .Include(q => q.Country)
            .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return hotel;
    }

    public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto createDto)
    {
        // var hotel = new Hotel
        // {
        //     Name = createDto.Name,
        //     Address = createDto.Address,
        //     Rating = createDto.Rating,
        //     CountryId = createDto.CountryId
        // };
        var hotel = mapper.Map<Hotel>(createDto);

        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();

        var countryName = await context.Countries
            .Where(c => c.CountryId == createDto.CountryId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync() ?? string.Empty;

        // return new GetHotelDto(
        //     hotel.Id,
        //     hotel.Name,
        //     hotel.Address,
        //     hotel.Rating,
        //     countryName 
        // );
        return mapper.Map<GetHotelDto>(hotel);
    }

    public async Task UpdateHotelAsync(int id, UpdateHotelDto updateDto)
    {
        var hotel = await context.Hotels.FindAsync(id)
            ?? throw new KeyNotFoundException("Hotel not found");

        hotel.Name = updateDto.Name;
        hotel.Address = updateDto.Address;
        hotel.Rating = updateDto.Rating;
        hotel.CountryId = updateDto.CountryId;

        await context.SaveChangesAsync();
    }

    public async Task DeleteHotelAsync(int id)
    {
        var hotel = await context.Hotels.FindAsync(id)
            ?? throw new KeyNotFoundException("Hotel not found");

        context.Hotels.Remove(hotel);
        await context.SaveChangesAsync();
    }

    public async Task<bool> HotelExistsAsync(int id)
    {
        return await context.Hotels.AnyAsync(e => e.Id == id);
    }
}
