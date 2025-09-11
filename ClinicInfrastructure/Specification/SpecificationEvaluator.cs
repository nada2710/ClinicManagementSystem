using ClinicDomain.Entities.Base;
using ClinicDomain.Interfaces.ISpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicInfrastructure.Specification
{
    public class SpecificationEvaluator<TEntity,Tkey> where TEntity:BaseEntity<Tkey>
    {
        public static IQueryable<TEntity>GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity,Tkey> specification)
        {
            var query = inputQuery.AsQueryable();//It takes the DbSet from the DbContext (for example, dbContext.Products) and makes it Queryable, so that we can build LINQ queries on top of it.
            if(specification.Criteria!=null)
            {
                query=query.Where(specification.Criteria);
            }
            //paging
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }
            //Navigation Properties
            query=specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            
            if(specification.OrderBy!=null)
            {
                query=query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            return query;
        }

    }
}
