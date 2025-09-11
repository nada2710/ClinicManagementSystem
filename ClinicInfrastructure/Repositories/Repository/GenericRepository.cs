using ClinicDomain.Entities.Base;
using ClinicDomain.Interfaces.IRepository;
using ClinicInfrastructure.DbHelper.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicInfrastructure.Repositories.Repository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly ClinicDbContext _clinicDbContext;
        private readonly DbSet<TEntity> _entities;

        public GenericRepository(ClinicDbContext clinicDbContext)
        {
            _clinicDbContext=clinicDbContext;
            _entities=_clinicDbContext.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _entities.FindAsync(id);
        }
        public async Task AddAsync(TEntity entity)
        {
             await _entities.AddAsync(entity);
        }
        public void Delete(TEntity entity)
        {
            if(entity!=null)
            {
                _entities.Remove(entity);
            }
        }
        public void Update(TEntity entity)
        {
            _entities.Update(entity);
            /*
             * it is the same of  _entities.Update(entity); but if i wanot to change one att. use it =>
             * _entities..Attach(entity);
             *  _context.Entry(entity).State = EntityState.Modified;
             */
        }
    }
}
