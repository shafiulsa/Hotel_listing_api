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
           .ForMember(destination => destination.Country, cfg => cfg.MapFrom(source => source.Country!.Name));

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
