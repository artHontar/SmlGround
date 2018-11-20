using System.Collections.Generic;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using System.Data.Entity;
using System.Linq;

namespace SmlGround.DataAccess.Repositories
{
    class ProfileRepository : IProfileRepository
    {

        public SocialDbContext Database { get; }

        public ProfileRepository(SocialDbContext db)
        {
            Database = db;
        }

        public void Delete(string id)
        {
            Profile profile = Database.Profiles.Find(id);
            if (profile != null)
                Database.Profiles.Remove(profile);
        }

        public void Create(Profile item)
        {
            Database.Profiles.Add(item);
        }

        public Profile GetProfile(string id)
        {
            return Database.Profiles.Find(id);
        }

        public List<Profile> GetAllProfiles()
        {
            return Database.Profiles.ToList();
        }

        public void Update(Profile item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
