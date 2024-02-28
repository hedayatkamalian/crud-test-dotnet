using AutoMapper;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}
