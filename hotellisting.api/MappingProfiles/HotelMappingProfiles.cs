using AutoMapper;
using hotellisting.api.data;
using hotellisting.api.DTOs.Country;
using hotellisting.api.DTOs.Hotel;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace hotellisting.api.MappingProfiles;

public class HotelMappingProfiles : Profile
{
    public HotelMappingProfiles()
    {
        CreateMap<Hotel, GetHotelDto>()
           .ForMember(destination => destination.Country, cfg => cfg.MapFrom<CountryNameResolver>());

        CreateMap<CreateHotelDto, Hotel>();
    }
}


public class CountryMappingProfiles : Profile
{
    public CountryMappingProfiles()
    {
        CreateMap<Country, GetCountryDto>();
        CreateMap<Country, GetCountriesDto>();
        CreateMap<CreateCountryDto, Country>(); 
    }
}

public class CountryNameResolver : IValueResolver<Hotel, GetHotelDto, string>
{
    public string Resolve(Hotel source, GetHotelDto destination, string destMember, ResolutionContext context)
    {
        return source.Country?.Name ?? string.Empty;
    }
}