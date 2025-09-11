using ClinicDomain.Entities.Base;
using ClinicDomain.Interfaces.ISpecification;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDomain.Interfaces.IRepository
{
    public interface IGenericRepository<TEntity,Tkey>where TEntity:BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Tkey id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        //Specification methods
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity,Tkey> specification);
        Task<TEntity> GetByIdWithSpecAsync(ISpecification<TEntity, Tkey> specification);
        //paging
        Task<int> CountAsync(ISpecification<TEntity, Tkey> spec);
    }
}
