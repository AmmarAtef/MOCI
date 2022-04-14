using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Linq.Dynamic;

namespace MOCI.DAL.Repositories
{
    public class GenericRepository<T, Y> : IGenericRepository<T, Y> where T : class
    {
        protected readonly MTRSDBContext _context;

        public GenericRepository(MTRSDBContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            var query = _context.Set<T>().AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                if (isSortAscending)
                {
                    var propertyInfo = typeof(T).GetProperty(sortColumn);
                    if (propertyInfo != null)
                    {
                        var param = Expression.Parameter(typeof(T));
                        var expr = Expression.Lambda<Func<T, object>>(
                            Expression.Convert(Expression.Property(param, propertyInfo), typeof(object)),
                            param
                        );
                        query = query.OrderBy(expr);
                    }
                }
                else
                {
                    var propertyInfo = typeof(T).GetProperty(sortColumn);
                    if (propertyInfo != null)
                    {
                        var param = Expression.Parameter(typeof(T));
                        var expr = Expression.Lambda<Func<T, object>>(
                            Expression.Convert(Expression.Property(param, propertyInfo), typeof(object)),
                            param
                        );
                        query = query.OrderByDescending(expr);
                    }
                }
            }

            if (page <= 0)
                page = 1;
            if (pageSize <= 0)
                pageSize = 20;

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return query.AsEnumerable();
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
       
        public T GetById(Y id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }
        public void Update()
        {
            _context.SaveChanges();
        }
        public int Count()
        {
            return _context.Set<T>().Count();
        }
    }
}
