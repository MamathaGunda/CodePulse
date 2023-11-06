using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAsync()
        {
            //return await _context.BlogPosts.ToListAsync(); //without categories 
            return await _context.BlogPosts.Include(x => x.Categories).ToListAsync(); //with categories domain model items
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await _context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogpost = await _context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogpost == null)
            {
                return null;
            }
            //updating the blogposts dB
            _context.BlogPosts.Entry(existingBlogpost).CurrentValues.SetValues(blogPost);
            //updating the categories dB
            existingBlogpost.Categories = blogPost.Categories;
            await _context.SaveChangesAsync();
            return blogPost;

        }
        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(existingBlogPost != null)
            {
                _context.BlogPosts.Remove(existingBlogPost);
                await _context.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }
    }
}
