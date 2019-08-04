using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.WebPages;
using aspnetmvc_starter.Core.Repositories;
using aspnetmvc_starter.Dtos;
using aspnetmvc_starter.Helpers;
using aspnetmvc_starter.Models;

namespace aspnetmvc_starter.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(DbContext context)
        {
            Context = context;
            _entities = Context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            // Note that here I've repeated Context.Set<TEntity>() in every method and this is causing
            // too much noise. I could get a reference to the DbSet returned from this method in the 
            // constructor and store it in a private field like _entities. This way, the implementation
            // of our methods would be cleaner:
            // 
            // _entities.ToList();
            // _entities.Where();
            // _entities.SingleOrDefault();
            // 
            // I didn't change it because I wanted the code to look like the videos. But feel free to change
            // this on your own.
            return Context.Set<TEntity>().ToList();
        }
        
        public IEnumerable<TEntity> Grid(HttpRequestBase Request)
        {
            var skip = 0;
            var pageSize = 0;

            IEnumerable<TEntity> query = Context.Set<TEntity>();

            query = GridFilterAndOrder(out skip, out pageSize, query, Request);

            return query.Skip(skip).Take(pageSize).ToList();
        }

        private static IEnumerable<TEntity> GridFilterAndOrder(out int skip, out int pageSize, IEnumerable<TEntity> query, HttpRequestBase Request)
        {
            var take = 0;
            var page = 0;
            string OrderByField = "";
            string OrderByType = "";
            List<KendoFilter> filters = new List<KendoFilter>();
            
            KendoGrid.GridParams(out take, out skip, out page, out pageSize, out OrderByField, out OrderByType, out filters, Request);
            
            
            if (!String.IsNullOrEmpty(OrderByField) && !String.IsNullOrEmpty(OrderByType))
            {
                query = (OrderByType == "desc")
                    ? query.OrderByDescending(o => o.GetProperty(OrderByField))
                    : query.OrderBy(o => o.GetProperty(OrderByField));
            }

            foreach (var filter in filters)
            {
                if (filter.Operator == "eq")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() == filter.Value);
                }
                else if (filter.Operator == "neq")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() != filter.Value);
                }
                else if (filter.Operator == "startswith")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString().StartsWith(filter.Value));
                }
                else if (filter.Operator == "endswith")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString().EndsWith(filter.Value));
                }
                else if (filter.Operator == "contains")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null &&
                        o.GetProperty(filter.Name).ToString().ToLower().Contains(filter.Value.ToLower()));
                }
                else if (filter.Operator == "doesnotcontain")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null &&
                        !o.GetProperty(filter.Name).ToString().ToLower().Contains(filter.Value.ToLower()));
                }
                else if (filter.Operator == "isnull")
                {
                    query = query.Where(
                        o => o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() == null);
                }
                else if (filter.Operator == "isnotnull")
                {
                    query = query.Where(
                        o => o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() != null);
                }
                else if (filter.Operator == "isempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString().IsEmpty());
                }
                else if (filter.Operator == "isnotempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && !o.GetProperty(filter.Name).ToString().IsEmpty());
                }
                else if (filter.Operator == "isnullorempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && String.IsNullOrEmpty(o.GetProperty(filter.Name).ToString()));
                }
                else if (filter.Operator == "isnotnullorempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && !String.IsNullOrEmpty(o.GetProperty(filter.Name).ToString()));
                }
            }

            return query;
        }


        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}