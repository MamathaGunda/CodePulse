﻿using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCat = await _context.Categories.FirstOrDefaultAsync(x=>x.Id==category.Id);

            if(existingCat != null)
            {
                //existingCat.Name = category.Name;
                //existingCat.UrlHandle = category.UrlHandle;
                _context.Categories.Entry(existingCat).CurrentValues.SetValues(category);
                await _context.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCat = await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if(existingCat == null)
            {
               return null;
            }
            _context.Categories.Remove(existingCat);
            await _context.SaveChangesAsync();
            return existingCat;
        }
    }
}
