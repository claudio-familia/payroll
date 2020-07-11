using Microsoft.EntityFrameworkCore;
using PayrollApp.Models;
using PayrollApp.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PayrollApp.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private PayrollContext _context;
        private DbSet<TEntity> _entity;

        public BaseRepository(PayrollContext context)
        {
            _context = context;
            _entity = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(object Id)
        {
            return await _entity.FindAsync(Id);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(TEntity entity)
        {
            _entity.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _entity.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
