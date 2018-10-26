using System.Threading.Tasks;
using Challenge.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge.API.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task<Contact> Delete(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            if(contact == null)
                return null;
            
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Contact> Get(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            if(contact == null)
                return null;

            return contact;
        }

        public async Task<Contact[]> GetByCountryOrCity(string countryOrCity)
        {
            var contactList = await _context.Contacts.ToListAsync();

            var contactsFiltered = contactList.FindAll(x => x.City.ToLower().Contains(countryOrCity.ToLower()) || x.Country.ToLower().Contains(countryOrCity.ToLower()));

            return contactsFiltered.ToArray();
        }

        public async Task<Contact[]> GetByPhoneOrEmail(string phoneEmail)
        {
            var contactList = await _context.Contacts.ToListAsync();

            var contactsFiltered = contactList.FindAll(x => x.PersonalPhoneNumber.ToLower().Contains(phoneEmail.ToLower()) || x.WorkPhoneNumber.Contains(phoneEmail) || x.Email.ToLower().Contains(phoneEmail.ToLower()));

            return contactsFiltered.ToArray();
        }

        public async Task<Contact> Save(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Contact> Update(Contact contact)
        {
             _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Photo> AddPhoto(Photo photo)
        {
            var contact = _context.Contacts.Find(photo.ContactId);
            contact.PhotoUrl = photo.Url;

            //var photoToRemove = await _context.Photos.FirstOrDefaultAsync(x => x.ContactId == photo.ContactId);
            
            //if(photoToRemove != null)
            //    _context.Photos.Remove(photoToRemove);

            _context.Photos.Add(photo);
            _context.Contacts.Update(contact);

            _context.SaveChanges();

            return photo;
        }

        public async Task<bool> ContactExists(string name, string company)
        {
            if(await _context.Contacts.AnyAsync(x => (x.Name == name && x.Company == company)))
                return true;
            
            return false;
        }

        public async Task<bool> ContactExists(int id, string name, string company)
        {
            if(await _context.Contacts.AnyAsync(x => (x.Id != id && x.Name == name && x.Company == company)))
                return true;
            
            return false;
        }

        public async Task<bool> ContactExists(int id)
        {
            if(await _context.Contacts.AnyAsync(x => x.Id == id))
                return true;
            
            return false;
        }
    }
}
