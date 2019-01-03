using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Repositories
{
    class DialogRepository : IRepository<Dialog,Int64>
    {
        public SocialDbContext Database { get; }

        public DialogRepository(SocialDbContext db)
        {
            Database = db;
        }

        public void Create(Dialog item)
        {
            Database.Set<Dialog>().Add(item);
            Save();
        }

        public IEnumerable<Dialog> GetAll()
        {
            return Database.Set<Dialog>().ToList();

        }

        public Dialog Get(Int64 idBy)
        {
            return Database.Set<Dialog>().Find(idBy);
        }

        public int Update(Dialog item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return Save();
        }

        public void Delete(Int64 idBy)
        {
            Dialog item = Database.Set<Dialog>().Find(idBy);
            if (item != null)
            {
                Database.Dialogs.Remove(item);
                Save();
            }
        }

        public async Task CreateAsync(Dialog item)
        {
            Database.Set<Dialog>().Add(item);
            await SaveAsync();
        }

        public async Task<IEnumerable<Dialog>> GetAllAsync()
        {
            return await Database.Set<Dialog>().ToListAsync();
        }

        public async Task<Dialog> GetAsync(Int64 idBy)
        {
            return await Database.Set<Dialog>().FindAsync(idBy);
        }

        public async Task<int> UpdateAsync(Dialog item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return await SaveAsync();
        }

        public async Task DeleteAsync(Int64 idBy)
        {
            Dialog item = Database.Dialogs.Find(idBy);
            if (item != null)
            {
                Database.Set<Dialog>().Remove(item);
            }
            await SaveAsync();
        }

        public int Save()
        {
            return Database.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Database.SaveChangesAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
