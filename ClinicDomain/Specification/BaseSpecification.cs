using ClinicDomain.Entities.Base;
using ClinicDomain.Interfaces.ISpecification;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDomain.Specification
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagingEnabled { get; set; }

        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<TEntity, bool>> criteria)//You can define the Criteria (the filter condition) when you create the specification
        {
            Criteria = criteria;
        }
        //helper methods
        protected void AddInclude(Expression<Func<TEntity,object>> includeExpression)//navigation properties
        {
            Includes.Add(includeExpression);
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy)//sorting
        {
            OrderBy=orderBy;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)//sorting
        {
            OrderByDescending = orderByDescending;
        }
        protected void ApplyPaging(int skip, int take)//paging
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
