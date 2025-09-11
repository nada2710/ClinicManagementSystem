using ClinicDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDomain.Interfaces.ISpecification
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        /*
         * fun<in,out>
         * Expression Tree. => ef convert Expression Tree to SQL.
         * (where) filter what i wont to do in the query
         */
        Expression<Func<TEntity, bool>> Criteria { get; }//return bool
        List<Expression<Func<TEntity, object>>> Includes { get; }//return object
        Expression<Func<TEntity,object>> OrderBy { get; set; }
        Expression<Func<TEntity, object>> OrderByDescending { get; set; }

        //Paging
        int Skip { get; set; }
        int Take { get; set;}
        bool IsPagingEnabled { get; set; }
    }
}
