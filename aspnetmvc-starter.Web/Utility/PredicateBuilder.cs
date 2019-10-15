using System;
using System.Linq;
using System.Linq.Expressions;

namespace aspnetmvc_starter.Web.Utility
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Make<T>() { return null; }

        public static Expression<Func<T, bool>> Make<T>(this Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> or)
        {
            if (expr == null) return or;
            var invokedExpr = Expression.Invoke(or, expr.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expr.Body, invokedExpr), expr.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> and)
        {
            if (expr == null) return and;
            var invokedExpr = Expression.Invoke(and, expr.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expr.Body, invokedExpr), expr.Parameters);
        }

        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
    }
}