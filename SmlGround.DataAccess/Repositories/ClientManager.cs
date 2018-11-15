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
    class ClientManager : IClientManager
    {
        public SocialDbContext Database { get; set; }

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
