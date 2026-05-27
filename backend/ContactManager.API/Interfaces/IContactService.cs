using ContactManager.API.DTOs;

namespace ContactManager.API.Interfaces;

public interface IContactService
{
    Task<PagedResult<ContactDto>> GetAllAsync(ContactQueryParams queryParams);
    Task<ContactDto?> GetByIdAsync(int id);
    Task<ContactDto> CreateAsync(CreateContactDto dto);
    Task<ContactDto?> UpdateAsync(int id, UpdateContactDto dto);
    Task<bool> DeleteAsync(int id);
}
