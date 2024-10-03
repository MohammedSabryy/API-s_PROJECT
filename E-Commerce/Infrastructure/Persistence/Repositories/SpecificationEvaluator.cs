using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, Specifications<T> specifications) where T : class
        {
            var query = inputQuery;
            if (specifications.Criteria != null) query = query.Where(specifications.Criteria);

            //foreach (var item in specifications.IncludeExpressions)
            //{
            //    query = query.Include(item);
            //}

            query = specifications.IncludeExpressions.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
