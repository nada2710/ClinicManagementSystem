using ClinicDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDomain.Interfaces.IRepository
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
