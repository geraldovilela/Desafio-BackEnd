using AutoMapper;
using RentalApp.Core.DTOs.Requests;
using RentalApp.Core.Entities;

namespace RentalApp.Core.AutoMapper
{
    public class RenterProfile : Profile
    {
        public RenterProfile()
        {

            CreateMap<Renter, RegisterRenterRequest>()
                    .ForMember(dest => dest.Identificador, opt => opt.MapFrom(src => src.RenterId))
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.CompanyRegistrationNumber))
                    .ForMember(dest => dest.Data_nascimento, opt => opt.MapFrom(src => src.Birthdate))
                    .ForMember(dest => dest.NumeroCnh, opt => opt.MapFrom(src => src.DriverLicenseNumber))
                    .ForMember(dest => dest.ImagemCnh, opt => opt.MapFrom(src => src.DriverLicenseImgString))
                    .ForMember(dest => dest.TipoCnh, opt => opt.MapFrom(src => src.DriverLicenseType));

            CreateMap<RegisterRenterRequest, Renter>()
                    .ForMember(dest => dest.RenterId, opt => opt.Ignore())
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nome))
                    .ForMember(dest => dest.CompanyRegistrationNumber, opt => opt.MapFrom(src => src.Cnpj))
                    .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Data_nascimento))
                    .ForMember(dest => dest.DriverLicenseNumber, opt => opt.MapFrom(src => src.NumeroCnh))
                    .ForMember(dest => dest.DriverLicenseImgString, opt => opt.MapFrom(src => src.ImagemCnh))
                    .ForMember(dest => dest.DriverLicenseType, opt => opt.MapFrom(src => src.TipoCnh));

        }
    }
}