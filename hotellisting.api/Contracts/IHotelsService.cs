using hotellisting.api.DTOs.Hotel;

namespace hotellisting.api.Contracts;

public interface IHotelsService
{
        Task<IEnumerable<GetHotelsDto>> GetHotelsAsync();
    Task<GetHotelDto?> GetHotelAsync(int id);
    Task<GetHotelDto> CreateHotelAsync(CreateHotelDto createDto);
    Task UpdateHotelAsync(int id, UpdateHotelDto updateDto);
    Task DeleteHotelAsync(int id);

    Task<bool> HotelExistsAsync(int id);
}
