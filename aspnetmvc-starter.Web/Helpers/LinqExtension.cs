using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace aspnetmvc_starter.Web.Helpers
{
    public static class LinqExtension
    {

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return (IQueryable<T>)OrderBy((IQueryable)source, propertyName);
        }
        
        public static IQueryable OrderBy(this IQueryable source, string propertyName)
        {
            var p = Expression.Parameter(source.ElementType, "p");
        
            LambdaExpression selector = Expression.Lambda(Expression.PropertyOrField(p, propertyName), p);
        
        
            var query = source.Provider.CreateQuery(
        
                Expression.Call(typeof(Queryable),
                            "OrderBy",
                            new Type[] { source.ElementType, selector.Body.Type },
                            source.Expression,
                            selector
                ));
        
            return query;
        }
        
        public static IQueryable OrderByDescending(this IQueryable source, string propertyName)
        {
            var p = Expression.Parameter(source.ElementType, "p");
        
            LambdaExpression selector = Expression.Lambda(Expression.PropertyOrField(p, propertyName), p);
        
        
            var query = source.Provider.CreateQuery(
        
                Expression.Call(typeof(Queryable),
                    "OrderByDescending",
                    new Type[] { source.ElementType, selector.Body.Type },
                    source.Expression,
                    selector
                ));
        
            return query;
        }


    }
}