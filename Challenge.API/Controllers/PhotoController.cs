using System.Threading.Tasks;
using Challenge.API.Data;
using Challenge.API.Dtos;
using Challenge.API.Helpers;
using Challenge.API.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Challenge.API.Controllers
{
    [Route("api/Contacts/{contactId}/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotoController(IRepository repo, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _repo = repo;

            Account acc = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForContact(int contactId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if(!await _repo.ContactExists(contactId))
                return StatusCode(404);

            var contact = _repo.Get(contactId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = new Photo()
            {
                Url = photoForCreationDto.Url,
                Description = photoForCreationDto.Description,
                DateAdded = photoForCreationDto.DateAdded,
                PublicId = photoForCreationDto.PublicId,
                ContactId = contactId
            };

            _repo.AddPhoto(photo);

            return Ok(photo);
        }
    }
}