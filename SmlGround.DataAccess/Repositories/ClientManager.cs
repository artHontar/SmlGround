using System.Collections.Generic;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using System.Data.Entity;
using System.Linq;

namespace SmlGround.DataAccess.Repositories
{
    class ClientManager : IClientManager
    {

        public SocialDbContext Database { get; }

        public ClientManager(SocialDbContext db)
        {
            Database = db;
        }

        public void Create(Profile item)
        {
            Database.Profiles.Add(item);
            Database.SaveChanges();
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
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
