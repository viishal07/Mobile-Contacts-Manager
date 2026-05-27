using AutoMapper;
using ContactManager.API.DTOs;
using ContactManager.API.Interfaces;
using ContactManager.API.Models;

namespace ContactManager.API.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactService> _logger;

    public ContactService(
        IContactRepository repository,
        IMapper mapper,
        ILogger<ContactService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<ContactDto>> GetAllAsync(ContactQueryParams queryParams)
    {
        _logger.LogInformation(
            "Fetching contacts. Page: {Page}, Search: {Search}",
            queryParams.Page,
            queryParams.Search);

        var pagedContacts = await _repository.GetAllAsync(queryParams);

        var dtoItems = _mapper.Map<List<ContactDto>>(pagedContacts.Items);

        return new PagedResult<ContactDto>
        {
            Items = dtoItems,
            TotalCount = pagedContacts.TotalCount,
            Page = pagedContacts.Page,
            PageSize = pagedContacts.PageSize
        };
    }

    public async Task<ContactDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation(
            "Fetching contact with ID: {Id}",
            id);

        var contact = await _repository.GetByIdAsync(id);

        return contact == null
            ? null
            : _mapper.Map<ContactDto>(contact);
    }

    public async Task<ContactDto> CreateAsync(CreateContactDto dto)
    {
        _logger.LogInformation(
            "Creating contact: {FirstName} {LastName}",
            dto.FirstName,
            dto.LastName);

        var contact = _mapper.Map<Contact>(dto);

        contact.CreatedAt = DateTime.UtcNow;

        var created = await _repository.CreateAsync(contact);

        return _mapper.Map<ContactDto>(created);
    }

    public async Task<ContactDto?> UpdateAsync(int id, UpdateContactDto dto)
    {
        _logger.LogInformation(
            "Updating contact with ID: {Id}",
            id);

        var contact = _mapper.Map<Contact>(dto);

        var updated = await _repository.UpdateAsync(id, contact);

        return updated == null
            ? null
            : _mapper.Map<ContactDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation(
            "Deleting contact with ID: {Id}",
            id);

        return await _repository.DeleteAsync(id);
    }
}