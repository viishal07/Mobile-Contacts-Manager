using ContactManager.API.Data;
using ContactManager.API.DTOs;
using ContactManager.API.Interfaces;
using ContactManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.API.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Contact>> GetAllAsync(ContactQueryParams queryParams)
    {
        var query = _context.Contacts.AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.ToLower();

            query = query.Where(c =>
                c.FirstName.ToLower().Contains(search) ||
                (c.LastName != null && c.LastName.ToLower().Contains(search)) ||
                (c.Email != null && c.Email.ToLower().Contains(search)) ||
                (c.PhoneNumber != null && c.PhoneNumber.Contains(search)) ||
                (c.Company != null && c.Company.ToLower().Contains(search))
            );
        }

        // Favorite Filter
        if (queryParams.Favorite.HasValue)
        {
            query = query.Where(c =>
                c.Favorite == queryParams.Favorite.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync();

        return new PagedResult<Contact>
        {
            Items = items,
            TotalCount = totalCount,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();

        return contact;
    }

    public async Task<Contact?> UpdateAsync(int id, Contact contact)
    {
        var existing = await _context.Contacts.FindAsync(id);

        if (existing == null)
            return null;

        existing.FirstName = contact.FirstName;
        existing.LastName = contact.LastName;
        existing.Email = contact.Email;
        existing.PhoneNumber = contact.PhoneNumber;
        existing.Company = contact.Company;
        existing.Address = contact.Address;
        existing.Favorite = contact.Favorite;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact == null)
            return false;

        _context.Contacts.Remove(contact);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Contacts.AnyAsync(c => c.Id == id);
    }
}