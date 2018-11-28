﻿using System;
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
    class FriendRepository : IRepository<Friend>
    {
        public SocialDbContext Database { get; }

        public FriendRepository(SocialDbContext db)
        {
            Database = db;
        }

        public void Create(Friend item)
        {
            Database.Set<Friend>().Add(item);
            Save();
        }

        public IEnumerable<Friend> GetAll()
        {
            return Database.Set<Friend>().ToList();

        }

        public Friend Get(string id)
        {
            return Database.Set<Friend>().Find(id);
        }

        public int Update(Friend item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return Save();
        }

        public void Delete(string id)
        {
            Friend item = Database.Set<Friend>().Find(id);
            if (item != null)
            {
                Database.Friends.Remove(item);
                Save();
            }
        }

        public async Task CreateAsync(Friend item)
        {
            Database.Set<Friend>().Add(item);
            await SaveAsync();
        }

        public async Task<IEnumerable<Friend>> GetAllAsync()
        {
            return await Database.Set<Friend>().Include(o => o.UserBy).Include(o => o.UserTo).ToListAsync();
        }

        public async Task<Friend> GetAsync(string id)
        {
            return await Database.Set<Friend>().FindAsync(id);
        }

        public async Task<int> UpdateAsync(Friend item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return await SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            Friend item = Database.Friends.Find(id);
            if (item != null)
            {
                Database.Set<Friend>().Remove(item);
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
