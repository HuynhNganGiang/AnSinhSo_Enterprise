using AnSinhSo.Application.Citizens.DTOs;
using AnSinhSo.Application.Common.Mappings;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AutoMapper;

namespace AnSinhSo.Application.Citizens.Queries;

public class CitizenMapping : IMapFrom<Citizen>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Citizen, CitizenDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.Value))
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName.FirstName + " " + s.FullName.MiddleName + " " + s.FullName.LastName))
            .ForMember(d => d.CitizenNumber, opt => opt.MapFrom(s => s.CitizenNumber.Value))
            .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Gender.Id))
            .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber.Value))
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address.Street + ", " + s.Address.Ward + ", " + s.Address.District + ", " + s.Address.Province))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Value))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.Id));

        profile.CreateMap<Citizen, CitizenSummaryDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.Value))
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName.FirstName + " " + s.FullName.MiddleName + " " + s.FullName.LastName))
            .ForMember(d => d.CitizenNumber, opt => opt.MapFrom(s => s.CitizenNumber.Value))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.Id));
    }
}
