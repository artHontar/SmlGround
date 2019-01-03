using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Interface
{
    public interface IRepository<TType,PKType> : IDisposable
    {
        void Create(TType item);
        IEnumerable<TType> GetAll();
        TType Get(PKType id);
        int Update(TType item);
        void Delete(PKType id);
        Task CreateAsync(TType item);
        Task<IEnumerable<TType>> GetAllAsync();
        Task<TType> GetAsync(PKType id);
        Task<int> UpdateAsync(TType item);
        Task DeleteAsync(PKType id);
        int Save();
        Task<int> SaveAsync();
    }
}
