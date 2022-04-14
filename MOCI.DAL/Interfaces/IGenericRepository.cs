using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MOCI.DAL.Interfaces
{
    public interface IGenericRepository<T, Y> where T : class
    {
        T GetById(Y id);
        IEnumerable<T> GetAll();
      
        IEnumerable<T> Find(Expression<Func<T, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update();
        int Count();
    }
}
