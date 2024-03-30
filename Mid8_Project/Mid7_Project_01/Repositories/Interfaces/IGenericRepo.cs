using Mid7_Project_01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mid7_Project_01.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T :  class, new()
    {
        IEnumerable<T> GetAll(string include = "");
        T Get(Expression<Func<T, bool>> predicate, string include = "");
        void Insert(T item);
        void Update(T item);
        void Delete(Expression<Func<T, bool>> predicate);

        K ExecuteSqlSingle<K>(string sql) where K : class, new();
        IEnumerable<K> ExecuteSqlCollection<K>(string sql) where K: class, new ();
        void ExecuteCommand(string sql);
    }
}
