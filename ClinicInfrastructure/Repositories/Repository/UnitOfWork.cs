using ClinicDomain.Entities.Base;
using ClinicDomain.Interfaces.IRepository;
using ClinicInfrastructure.DbHelper.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicInfrastructure.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicDbContext _clinicDbContext;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(ClinicDbContext clinicDbContext,IServiceProvider serviceProvider)
        {
            _clinicDbContext=clinicDbContext;
            _serviceProvider=serviceProvider;
        }
        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            return _serviceProvider.GetRequiredService<IGenericRepository<TEntity, TKey>>();

        }
        public async Task SaveChangesAsync()
        {
            await _clinicDbContext.SaveChangesAsync();
        }
    }
}
