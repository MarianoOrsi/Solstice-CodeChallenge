using System.Threading.Tasks;
using Challenge.API.Data;
using Challenge.API.Dtos;
using Challenge.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Challenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IConfiguration _config;

        public ContactsController(IRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("AddContact")]
        public async Task<IActionResult> Save(ContactForSave contactForSave)
        {
            contactForSave.Name = contactForSave.Name.ToLower();
            contactForSave.Company = contactForSave.Company.ToLower();
            contactForSave.Street = contactForSave.Street.ToLower();
            contactForSave.Country = contactForSave.Country.ToLower();
            contactForSave.City = contactForSave.City.ToLower();
            contactForSave.Email = contactForSave.Email.ToLower();

            if(await _repo.ContactExists(contactForSave.Name, contactForSave.Company))
                return BadRequest("The contact already exists");
            
            var contactToCreate = new Contact
            {
                Name = contactForSave.Name,
                Company = contactForSave.Company,
                Email = contactForSave.Email,
                Birthdate = contactForSave.BirthDate,
                PersonalPhoneNumber = contactForSave.PersonalPhoneNumber,
                WorkPhoneNumber = contactForSave.WorkPhoneNumber,
                Street = contactForSave.Street,
                City = contactForSave.City,
                Country = contactForSave.Country
            };

            var createdContact = await _repo.Save(contactToCreate);

            return Ok(createdContact);
        }

        [HttpGet("GetContact/{contactId}")]
        public async Task<IActionResult> Get(int contactId)
        {
            var createdContact = await _repo.Get(contactId);

            if(createdContact == null)
                return NoContent();

            return Ok(createdContact);
        }

        [HttpDelete("DeleteContact/{contactId}")]
        public async Task<IActionResult> Delete(int contactId)
        {
            var contactDeleted = await _repo.Delete(contactId);

            if(contactDeleted == null)
                return NoContent();
            
            return Ok("Contact " + contactDeleted.Name + " Deleted!");
        }

        [HttpPut("UpdateContact")]
        public async Task<IActionResult> Update(ContactForUpdate contactForUpdate)
        {
            contactForUpdate.Name = contactForUpdate.Name.ToLower();
            contactForUpdate.Company = contactForUpdate.Company.ToLower();
            contactForUpdate.Street = contactForUpdate.Street.ToLower();
            contactForUpdate.City = contactForUpdate.City.ToLower();
            contactForUpdate.Country = contactForUpdate.Country.ToLower();
            contactForUpdate.Email = contactForUpdate.Email.ToLower();

            if(!await _repo.ContactExists(contactForUpdate.Id))
                return BadRequest("The contact not exists");

            if(await _repo.ContactExists(contactForUpdate.Id, contactForUpdate.Name, contactForUpdate.Company))
                return BadRequest("The contact with name " + contactForUpdate.Name + " and company " + contactForUpdate.Company + " already exists");
            
            var contactToUpdate = new Contact
            {
                Id = contactForUpdate.Id,
                Name = contactForUpdate.Name,
                Company = contactForUpdate.Company,
                Email = contactForUpdate.Email,
                Birthdate = contactForUpdate.BirthDate,
                PersonalPhoneNumber = contactForUpdate.PersonalPhoneNumber,
                WorkPhoneNumber = contactForUpdate.WorkPhoneNumber,
                Street = contactForUpdate.Street,
                City = contactForUpdate.City,
                Country = contactForUpdate.Country
            };

            var updatedContact = await _repo.Update(contactToUpdate);

            return Ok(updatedContact);
        }

        [HttpGet("GetByCityOrCountry/{term}")]
        public async Task<IActionResult> GetByCityOrCountry(string term)
        {
            var contactsFiltered = await _repo.GetByCountryOrCity(term);

            if(contactsFiltered == null)
                return NoContent();
            
            return Ok(contactsFiltered);
        }

        [HttpGet("GetByPhoneOrEmail/{term}")]
        public async Task<IActionResult> GetByPhoneOrEmail(string term)
        {
            var contactsFiltered = await _repo.GetByPhoneOrEmail(term);

            if(contactsFiltered == null)
                return NoContent();
            
            return Ok(contactsFiltered);
        }
    }
}