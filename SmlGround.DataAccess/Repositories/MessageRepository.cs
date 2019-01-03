using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Repositories
{
    class MessageRepository : IRepository<Message,Int64>
    {
        public SocialDbContext Database { get; }

        public MessageRepository(SocialDbContext db)
        {
            Database = db;
        }

        public void Create(Message item)
        {
            Database.Set<Message>().Add(item);
            Save();
        }

        public IEnumerable<Message> GetAll()
        {
            return Database.Set<Message>().ToList();

        }

        public Message Get(Int64 idBy)
        {
            return Database.Set<Message>().Find(idBy);
        }

        public int Update(Message item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return Save();
        }

        public void Delete(Int64 idBy)
        {
            Message item = Database.Set<Message>().Find(idBy);
            if (item != null)
            {
                Database.Messages.Remove(item);
                Save();
            }
        }

        public async Task CreateAsync(Message item)
        {
            Database.Set<Message>().Add(item);
            await SaveAsync();
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await Database.Set<Message>().ToListAsync();
        }

        public async Task<Message> GetAsync(Int64 idBy)
        {
            return await Database.Set<Message>().FindAsync(idBy);
        }

        public async Task<int> UpdateAsync(Message item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return await SaveAsync();
        }

        public async Task DeleteAsync(Int64 idBy)
        {
            Message item = Database.Messages.Find(idBy);
            if (item != null)
            {
                Database.Set<Message>().Remove(item);
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
