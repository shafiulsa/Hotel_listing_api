using hotellisting.api.DTOs.Hotel;

namespace hotellisting.api.DTOs.Country;

public record GetCountryDto(
    int Id,
    string Name,
    string ShortName,
    List<GetHotelSlimDto> Hotels
);

public record GetCountriesDto(
    int Id,
    string Name,
    string ShortName
);