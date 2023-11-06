using Azure;
using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTOs;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;    
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            //Map domain model to DTO
            var response = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDTO { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCat = await _categoryRepository.GetById(id);
            if (existingCat == null)
            {
                return NotFound();
            }
            var response = new CategoryDTO { Id = existingCat.Id, Name = existingCat.Name, UrlHandle = existingCat.UrlHandle };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO request)
        {
            //Map DTO to domain model
            var category = new Category { Name = request.Name, UrlHandle = request.UrlHandle };
            var c = await _categoryRepository.CreateAsync(category);
            //Map domain model back to DTO
            var response = new CategoryDTO { Id = c.Id, Name = c.Name, UrlHandle = c.UrlHandle };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var category = new Category { Id = id, Name = request.Name, UrlHandle= request.UrlHandle };
            category = await _categoryRepository.UpdateAsync(category);
            if(category == null)
            {
                return NotFound();
            }
            //convert domain model to DTO
            var response = new CategoryDTO { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _categoryRepository.DeleteAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            var response = new CategoryDTO { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle };
            return Ok(category);
        }
    }
}
