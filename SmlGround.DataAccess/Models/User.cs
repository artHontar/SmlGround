using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Posts = new List<Post>();
            Friends = new List<Friend>();
            Dialogs = new List<Dialog>();
        }


        public DateTime RegistrationTime { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<Dialog> Dialogs { get; set; }
        
        public Profile Profile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
