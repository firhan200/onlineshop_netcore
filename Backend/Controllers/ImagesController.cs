using DTOs.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.ImageRepository;
using Schema;

namespace Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Enums.Roles.Administrator))]
    public class ImagesController : ControllerBase {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGenericRepository<Image> _imageRepository;
        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

        public ImagesController(
            IWebHostEnvironment webHostEnvironment,
            IGenericRepository<Image> imageRepository
        ) {
            _webHostEnvironment = webHostEnvironment;
            _imageRepository = imageRepository;
        }

        [HttpPost(Name = "Upload Image")]
        public async Task<IResult> UploadImage(IFormFile image) {
            //validate
            if (image.Length <= 0)
            {
                return Results.BadRequest(new UploadImageDTO{
                    Success = false,
                    ErrorMessage = "Image is Empty"
                });
            }

            //get image ext
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            //validate image ext
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return Results.BadRequest(new UploadImageDTO{
                    Success = false,
                    ErrorMessage = "Invalid File Type"
                });
            }

            //get filename
            string fileName = Guid.NewGuid().ToString() + ext;
            string uploadDir = "images";
            string physicalPath = $"wwwroot/{uploadDir}";

            //generate physical file path
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName);

            //saving image
            using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);

            //create url path
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string fileUrlPath = $"{baseUrl}/{uploadDir}/{fileName}";

            //insert into database
            _imageRepository.Create(new Schema.Image{
                UrlPath = fileUrlPath,
                Extension = ext,
                Size = Convert.ToInt32(image.Length / 1000)
            });

            return Results.Ok(new UploadImageDTO{
                Success = true,
                UrlPath = fileUrlPath
            });
        }
    }
}