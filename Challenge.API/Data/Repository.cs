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

        public async Task<Contact> GetByCountryOrCity(string phoneEmail)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Contact> GetByPhoneOrEmail(string phoneEmail)
        {
            throw new System.NotImplementedException();
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

        public async Task<bool> ContactExists(string name, string company)
        {
            if(await _context.Contacts.AnyAsync(x => x.Name == name && x.Company == company))
                return true;
            
            return false;
        }

        public async Task<bool> ContactExistsById(int id)
        {
            if(await _context.Contacts.AnyAsync(x => x.Id == id))
                return true;
            
            return false;
        }
    }
}