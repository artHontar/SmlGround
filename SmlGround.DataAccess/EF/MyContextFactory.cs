using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.EF
{
    class MyContextFactory : IDbContextFactory<SocialDbContext>
    {
        public SocialDbContext Create()
        {
            return new SocialDbContext("DbConnection");
        }
    }
}
