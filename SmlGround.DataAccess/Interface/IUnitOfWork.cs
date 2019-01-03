using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmlGround.DataAccess.Identity;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IRepository<Profile,string> ProfileManager { get; }
        IRepository<Friend,string[]> FriendManager { get; }
        IRepository<Message, Int64> MessageManager { get; }
        IRepository<Dialog, Int64> DialogManager { get; }
        ApplicationRoleManager RoleManager { get; }
    }
}
