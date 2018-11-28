using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Interface
{
    public interface IRepository<T> : IDisposable
    {
        void Create(T item);
        IEnumerable<T> GetAll();
        T Get(string id);
        int Update(T profile);
        void Delete(string id);
        Task CreateAsync(T item);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task<int> UpdateAsync(T item);
        Task DeleteAsync(string id);
        int Save();
        Task<int> SaveAsync();
    }
}
