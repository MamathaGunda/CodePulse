using CodePulse.API.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        //[HttpPost]
        ////{apibaseurl}/api/images
        //public async Task<IActionResult> UploadImage([FromForm] IFormFile formFile, [FromForm] string fileName, [FromForm] string title)
        //{
        //    ValidateFileUpload(formFile);
        //    if(ModelState.IsValid)
        //    {
        //        var blogImage = new BlogImage
        //        {

        //        }
        //    }
        //}

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", "jpeg", "png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}
