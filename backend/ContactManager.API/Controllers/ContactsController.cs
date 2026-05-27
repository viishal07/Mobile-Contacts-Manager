using ContactManager.API.DTOs;
using ContactManager.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _service;
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(
        IContactService service,
        ILogger<ContactsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: api/contacts
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ContactQueryParams queryParams)
    {
        var result = await _service.GetAllAsync(queryParams);
        return Ok(result);
    }

    // GET: api/contacts/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var contact = await _service.GetByIdAsync(id);

        if (contact == null)
        {
            return NotFound(new
            {
                Message = $"Contact with ID {id} not found."
            });
        }

        return Ok(contact);
    }

    // POST: api/contacts
    [HttpPost]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContactDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var contact = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = contact.Id },
            contact);
    }

    // PUT: api/contacts/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateContactDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);

        if (updated == null)
        {
            return NotFound(new
            {
                Message = $"Contact with ID {id} not found."
            });
        }

        return Ok(updated);
    }

    // DELETE: api/contacts/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                Message = $"Contact with ID {id} not found."
            });
        }

        return NoContent();
    }
}