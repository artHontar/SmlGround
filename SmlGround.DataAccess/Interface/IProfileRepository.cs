using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Interface
{
    public interface IProfileRepository : IDisposable
    {
        void Create(Profile profile);
        void Update(Profile profile);
        void Delete(string id);
        List<Profile> GetAllProfiles();
        Profile GetProfile(string id);
    }
}
