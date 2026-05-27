using ContactManager.API.DTOs;
using ContactManager.API.Models;

namespace ContactManager.API.Interfaces;

public interface IContactRepository
{
    Task<PagedResult<Contact>> GetAllAsync(ContactQueryParams queryParams);
    Task<Contact?> GetByIdAsync(int id);
    Task<Contact> CreateAsync(Contact contact);
    Task<Contact?> UpdateAsync(int id, Contact contact);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
