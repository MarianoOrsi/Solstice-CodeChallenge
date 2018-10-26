using System.Threading.Tasks;
using Challenge.API.Models;

namespace Challenge.API.Data
{
    public interface IRepository
    {
         Task<Contact> Save(Contact contact);
         Task<Contact> Update(Contact contact);
         Task<Contact> Delete(int id);
         Task<Contact> Get(int id);
         Task<Contact[]> GetByPhoneOrEmail(string phoneEmail);
         Task<Contact[]> GetByCountryOrCity(string phoneEmail);
         Task<bool> ContactExists(string name, string company);
         Task<bool> ContactExists(int id);
         Task<bool> ContactExists(int id, string name, string company);
         Task<Photo> AddPhoto(Photo photo);
    }
}