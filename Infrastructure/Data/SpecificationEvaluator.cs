using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities;
using core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec){
            var query = inputQuery;
            if(spec.Criteria != null) query = query.Where(spec.Criteria); //used for filtering/Searching

             if(spec.OrderBy != null) query = query.OrderBy(spec.OrderBy); //used for sorting ASC

              if(spec.OrderByDescending != null) query = query.OrderByDescending(spec.OrderByDescending); //used for sorting Desc

              if(spec.IsPaginationEnabled) query = query.Skip(spec.Skip).Take(spec.Take); //Used for Pagination

            query = spec.Includes.Aggregate(query,(current , include) => current.Include(include));
            return query;
            
        }
    }
}