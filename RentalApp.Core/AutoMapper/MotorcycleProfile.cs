using AutoMapper;
using RentalApp.Core.DTOs.Requests;
using RentalApp.Core.DTOs.Responses;
using RentalApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Core.AutoMapper
{
    public class MotorcycleProfile : Profile
    {
        public MotorcycleProfile()
        {
            CreateMap<Motorcycle, MotorcycleResponse>()
                .ForMember(dest => dest.Identificador, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.VehiclePlate));

            CreateMap<CreateMotorcycleRequest, Motorcycle>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identificador))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Ano))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Modelo))
                .ForMember(dest => dest.VehiclePlate, opt => opt.MapFrom(src => src.Placa));
        }
    }
}
