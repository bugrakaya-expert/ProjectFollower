using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ProjectFollower.DataAcces.IMainRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null);


        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
            string includeProperties = null);

        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        //Bu interface Repository classına implement ediyoruz. Ders 39

        //Düzeltme -----  Func<IQueryable<T>, IQueryable<T>> orderBy = null,
    }
}
