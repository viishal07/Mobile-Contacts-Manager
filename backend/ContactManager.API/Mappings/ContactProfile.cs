using AutoMapper;
using ContactManager.API.DTOs;
using ContactManager.API.Models;

namespace ContactManager.API.Mappings;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, ContactDto>();
        CreateMap<CreateContactDto, Contact>();
        CreateMap<UpdateContactDto, Contact>();
        CreateMap<PagedResult<Contact>, PagedResult<ContactDto>>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());
    }
}
