using System.Collections.Generic;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Repositories
{
    class ProfileRepository : IRepository<Profile>
    {
        public SocialDbContext Database { get; }
        
        public ProfileRepository(SocialDbContext db)
        {
            Database = db;
        }

        public void Create(Profile item)
        {
            Database.Profiles.Add(item);
            Save();
        }

        public IEnumerable<Profile> GetAll()
        {
            return Database.Profiles.ToList();
            
        }

        public Profile Get(string id)
        {
            return Database.Profiles.Find(id);
        }

        public int Update(Profile item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return Save();
        }

        public void Delete(string id)
        {
            Profile profile = Database.Profiles.Find(id);
            if (profile != null)
            {
                Database.Profiles.Remove(profile);
                Save();
            }
        }

        public async Task CreateAsync(Profile item)
        {
            Database.Set<Profile>().Add(item);
            await SaveAsync();
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await Database.Set<Profile>().ToListAsync();
        }

        public async Task<Profile> GetAsync(string id)
        {
            return await Database.Set<Profile>().FindAsync(id);
        }

        public async Task<int> UpdateAsync(Profile item)
        {
            Database.Entry(item).State = EntityState.Modified;
            return await SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            Profile profile = Database.Profiles.Find(id);
            if (profile != null)
            {
                Database.Set<Profile>().Remove(profile);
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
