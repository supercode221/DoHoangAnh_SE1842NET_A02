﻿using FUNewsManagement_CoreAPI.DAL.Data;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_CoreAPI.DAL.Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly FUNMSDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(FUNMSDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public int Count() => _dbSet.Count();
    }
}
