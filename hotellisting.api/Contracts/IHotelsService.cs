using hotellisting.api.DTOs.Hotel;
using hotellisting.api.Results;

namespace hotellisting.api.Contracts;

public interface IHotelsService
{
    Task<Result<IEnumerable<GetHotelDto>>> GetHotelsAsync();
    Task<Result<GetHotelDto>> GetHotelAsync(int id);
    Task<Result<GetHotelDto>> CreateHotelAsync(CreateHotelDto createDto);
    Task<Result> UpdateHotelAsync(int id, UpdateHotelDto updateDto);
    Task<Result> DeleteHotelAsync(int id);

    Task<bool> HotelExistsAsync(int id);
    Task<bool> HotelExistsAsync(string name, int countryId);
}
