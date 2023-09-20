using AutoMapper;
using CarAPI.Entities;
using CarAPI.Models;

namespace CarAPI.Mapper
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile()
        {
            CreateMap<Car, CarDto>()
                .ForMember(cd => cd.EngineHorsepower, c => c.MapFrom(d => d.Engine.Horsepower))
                .ForMember(cd => cd.EngineDisplacement, c => c.MapFrom(d => d.Engine.Displacement))
                .ForMember(cd => cd.FuelType, c => c.MapFrom(d => d.Engine.FuelType));

            CreateMap<TechnicalReview, TechnicalReviewDto>();
            CreateMap<Repair, RepairDto>();

            CreateMap<NewCarDto, Car>()
               .ForMember(c => c.Engine, n => n.MapFrom(dto => new Engine()
               {
                   Horsepower = dto.EngineHorsepower,
                   Displacement = dto.EngineDisplacement,
                   FuelType = dto.FuelType
               })).ForMember(c => c.OcInsurance, n => n.MapFrom(dto => new Insurance()
               {
                   StartDate = dto.OcInsuranceStartDate,
                   EndDate = dto.OcInsuranceEndDate,
                   PolicyNumber = dto.OcPolicyNumber,
               }));

            CreateMap<NewTechnicalReviewDto,TechnicalReview>();
        }
    }
}
