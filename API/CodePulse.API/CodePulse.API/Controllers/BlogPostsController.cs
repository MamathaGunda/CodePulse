using Azure.Core;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTOs;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var blogposts = await _blogPostRepository.GetAsync();
            var response = new List<BlogPostDTO>();
            foreach (var blogPost in blogposts)
            {
                response.Add(new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageURL = blogPost.FeaturedImageURL,
                    IsVisible = blogPost.IsVisible,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageURL = blogPost.FeaturedImageURL,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDTO { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPostAsync([FromBody] CreateBlogPostRequestDTO request)
        {
            var blogPost = new BlogPost
            {
                Title = request.Title,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageURL = request.FeaturedImageURL,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };
            foreach (var catGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(catGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
            blogPost = await _blogPostRepository.CreateAsync(blogPost);
            //convert domain model to DTO
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageURL = blogPost.FeaturedImageURL,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDTO { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogpost([FromRoute] Guid id, [FromBody] UpdateBlogPostRequestDTO request )
        {
            //Convert DTO to domain model
            var blogPost = new BlogPost
            {
                Id= id,
                Title = request.Title,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageURL = request.FeaturedImageURL,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };
            foreach (var cat in request.Categories)
            {
                var existingCat = await _categoryRepository.GetById(cat);
                if(existingCat != null)
                {
                    blogPost.Categories.Add(existingCat);
                }
            }
            var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogPost);
            if(updatedBlogPost == null)
            {
                return NotFound();
            }
            //convert domain model to DTO
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageURL = blogPost.FeaturedImageURL,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDTO { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var deletedBlogPost = await _blogPostRepository.DeleteAsync(id);
            if(deletedBlogPost == null)
            {
                return NotFound();
            }
            var response = new BlogPostDTO
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                ShortDescription = deletedBlogPost.ShortDescription,
                UrlHandle = deletedBlogPost.UrlHandle,
                PublishedDate = deletedBlogPost.PublishedDate,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                FeaturedImageURL = deletedBlogPost.FeaturedImageURL,
                IsVisible = deletedBlogPost.IsVisible
            };
            return Ok(deletedBlogPost);
        }
    }
}
