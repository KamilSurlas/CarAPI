﻿using AutoMapper;
using CarAPI.Entities;
using CarAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace CarAPI.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);


            CreateMap<Car, CarDto>()
                .ForMember(cd => cd.EngineHorsepower, c => c.MapFrom(d => d.Engine.Horsepower))
                .ForMember(cd => cd.EngineDisplacement, c => c.MapFrom(d => d.Engine.Displacement))
                .ForMember(cd => cd.FuelType, c => c.MapFrom(d => d.Engine.FuelType));
                

            CreateMap<TechnicalReview, TechnicalReviewDto>();
            CreateMap<Repair, RepairDto>();
            CreateMap<Insurance,  InsuranceDto>();
            CreateMap<UpdateCarDto, Car>()
             .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));






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

            CreateMap<UpdateTechnicalReviewDto, TechnicalReview>()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<NewRepairDto, Repair>();
            CreateMap<UpdateRepairDto, Repair>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<NewInsuranceDto, Insurance>();
            CreateMap<UpdateInsuranceDto, Insurance>()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<RegisterUserDto, User>().ConvertUsing<RegisterUserConverter>();
               
        }
    }
}
